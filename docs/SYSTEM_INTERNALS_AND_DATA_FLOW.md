# Selqi Ecommerce Portal — System Internals and Data Flow

**Audience:** Technical architects, senior developers, technical business heads, integration teams  
**Purpose:** Explains what happens behind the screen — request flows, AI pipelines, auth mechanics, data relationships, and order lifecycle  
**Version:** 1.0  
**Last Updated:** May 2026  

---

## Table of Contents

1. [How the Frontend and Backend Communicate](#1-how-the-frontend-and-backend-communicate)
2. [Authentication Flow — Deep Dive](#2-authentication-flow--deep-dive)
3. [Product Search — What Happens Behind the Scenes](#3-product-search--what-happens-behind-the-scenes)
4. [The AI Recommendation Pipeline](#4-the-ai-recommendation-pipeline)
5. [The Checkout and Order Creation Flow](#5-the-checkout-and-order-creation-flow)
6. [Order Lifecycle and Status Machine](#6-order-lifecycle-and-status-machine)
7. [Password Reset Flow — Security Internals](#7-password-reset-flow--security-internals)
8. [Admin Action Audit Trail](#8-admin-action-audit-trail)
9. [Caching Strategy](#9-caching-strategy)
10. [Data Relationships Map](#10-data-relationships-map)
11. [Delivery Charge Calculation Logic](#11-delivery-charge-calculation-logic)

---

## 1. How the Frontend and Backend Communicate

### Overview

The Angular frontend and the ASP.NET Core backend are completely decoupled applications. They communicate exclusively through HTTP REST calls carrying JSON payloads.

```
┌──────────────────────────────────────────────────────────┐
│          Angular SPA (Browser)                           │
│                                                          │
│  Component → Service → HttpClient                        │
│                              ↕  HTTP + JSON              │
│             AuthInterceptor adds JWT header              │
└──────────────────────────────────────────────────────────┘
                       ↕
                   HTTPS REST
                   (port 5123)
┌──────────────────────────────────────────────────────────┐
│          ASP.NET Core API                                │
│                                                          │
│  Controller → Application Service → Repository → EF Core│
│                                               ↕          │
│                                           SQL Server     │
└──────────────────────────────────────────────────────────┘
```

### Request Example: Loading Products

```
1. User types "laptop" into the search box
2. Angular CatalogService calls:
   GET /api/v1/products?search=laptop&page=1&pageSize=24
3. HTTP request leaves browser with header:
   Authorization: Bearer eyJhbGci...  (if logged in)
4. ASP.NET Core middleware validates JWT → routes to CatalogController
5. CatalogController calls CatalogService.GetProductsAsync(query, ct)
6. CatalogService normalises input: "laptop".ToLower().Trim()
7. EfCatalogReadRepository executes LINQ query against SQL Server
8. Results mapped to List<ProductSummaryDto>
9. Wrapped in PagedResultDto<ProductSummaryDto>
10. Serialised to JSON and returned as 200 OK
11. Angular maps response to typed interface ProductSummary[]
12. Component signal updated → page re-renders with results
```

### JWT Injection (AuthInterceptor)

Every outgoing HTTP request passes through the Angular `AuthInterceptor`, which:
1. Reads the JWT from `localStorage` (key: `selqi_token`)
2. Appends `Authorization: Bearer <token>` to the request headers
3. If the response comes back as `401 Unauthorized`, calls `AuthService.logout()` which clears storage and redirects to `/login`

---

## 2. Authentication Flow — Deep Dive

### Registration

```
Frontend (RegisterComponent)
        ↓
POST /api/v1/auth/register
  Body: { email, password, firstName, lastName }
        ↓
AuthController → AuthService.RegisterAsync()
        ↓
1. Check if email already exists in Users table
   → 409 Conflict if duplicate
        ↓
2. Hash password:
   PBKDF2(password, salt, 100_000 iterations, SHA-256)
   → Produces PasswordHash string (includes salt)
        ↓
3. Create ApplicationUser entity with role "Customer"
        ↓
4. Save to Users table via EF Core
        ↓
5. Generate JWT:
   Claims: { sub: userId, email, given_name, family_name, role: "Customer" }
   Signed with HMAC-SHA256 using Jwt:Secret from config
   Expires: now + 1440 minutes
        ↓
6. Return AuthResponse { token, expiresAt, user: UserDto }
        ↓
Frontend
  → Stores token in localStorage["selqi_token"]
  → Stores UserDto in localStorage["selqi_user"]
  → Updates AuthService._user signal
  → isAuthenticated computed signal becomes true
  → Redirects to home
```

### Login

Same flow as registration from step 4, except:
- Step 1: Finds existing user by email (case-insensitive)
- Step 2: Verifies password against stored hash (constant-time comparison)
- Returns same `AuthResponse` shape

### JWT Validation (per request)

```
Incoming request with Authorization: Bearer <token>
        ↓
ASP.NET Core JWT middleware intercepts
        ↓
1. Decodes token (base64)
2. Verifies HMAC-SHA256 signature using Jwt:Secret
3. Checks Issuer = "SelqiApi"
4. Checks Audience = "SelqiClient"
5. Checks token not expired (exp claim)
        ↓
Valid: HttpContext.User populated with claims
Invalid / Missing: 401 Unauthorized returned
        ↓
Controller uses HttpContext.User.FindFirst("sub") to get userId
```

---

## 3. Product Search — What Happens Behind the Scenes

### Query Construction

When the user applies filters, the repository builds an `IQueryable<Product>` that is assembled conditionally:

```csharp
var query = _ctx.Products
    .Where(p => !p.IsDeleted)               // Always: exclude deleted
    .Include(p => p.Category)               // JOIN categories
    .Include(p => p.Brand)                  // JOIN brands
    .Include(p => p.Images)                 // JOIN images
    .Include(p => p.Inventory)              // JOIN stock
    .AsNoTracking();                        // Read-only: no change tracking

if (!string.IsNullOrEmpty(search))
    query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));

if (!string.IsNullOrEmpty(categorySlug))
    query = query.Where(p => p.Category.Slug == categorySlug);

if (minPrice.HasValue)
    query = query.Where(p => p.Price >= minPrice.Value);

if (availableOnly)
    query = query.Where(p => p.Inventory.Any(i => i.QuantityAvailable > 0));

// Count for pagination BEFORE applying Skip/Take
var totalItems = await query.CountAsync(ct);

// Apply sorting
query = sort switch {
    "price-asc"  => query.OrderBy(p => p.Price),
    "price-desc" => query.OrderByDescending(p => p.Price),
    "rating"     => query.OrderByDescending(p => p.Rating),
    _            => query.OrderByDescending(p => p.CreatedAtUtc)  // newest
};

// Apply pagination
var items = await query
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync(ct);
```

All of this is translated by EF Core into a single parameterised SQL query — no string concatenation, no SQL injection risk.

### Slug Resolution

When a user navigates to `/product/apple-airpods-pro`, the route parameter `apple-airpods-pro` is the product's `Slug` field. The repository queries `Products WHERE Slug = @slug AND IsDeleted = 0`. If no record is found, `null` is returned and the controller sends `404 Not Found`.

---

## 4. The AI Recommendation Pipeline

### Full Flow Diagram

```
GET /api/v1/recommendations
    (UserId extracted from JWT if authenticated, else null)
        ↓
GeminiRecommendationService.GetRecommendationsAsync(userId)
        ↓
STEP 1: CACHE CHECK
  cacheKey = "ai-recs:{userId ?? 'anon'}"
  IMemoryCache.TryGetValue(cacheKey, out result)
  → HIT: return immediately (30-min sliding TTL)
  → MISS: continue to step 2
        ↓
STEP 2: LOAD PRODUCT POOL
  SELECT TOP 50 products
  WHERE IsDeleted = 0
    AND Inventory has QuantityAvailable > 0
  ORDER BY CreatedAtUtc DESC
  → Mapped to simplified JSON: [{ slug, name, category, brand, price }]
        ↓
STEP 3: BUILD USER CONTEXT
  If userId not null:
    SELECT last 5 Orders with their OrderItems for this user
    → "This customer has previously purchased: {comma-separated product names}"
  Else:
    → "This is a new visitor with no purchase history."
        ↓
STEP 4: BUILD PROMPT
  """
  You are a product recommendation engine for an ecommerce platform.
  {userContext}
  From the following product pool, recommend exactly 6 products.
  Vary recommendations across different categories.
  Respond ONLY with a JSON array: [{"slug":"...","reason":"..."}]
  
  Product pool:
  {productPoolJson}
  """
        ↓
STEP 5: CALL GEMINI API
  POST https://generativelanguage.googleapis.com/v1beta/
       models/gemini-1.5-flash:generateContent?key={apiKey}
  Body: {
    "contents": [{ "parts": [{ "text": "<prompt>" }] }]
  }
  → Response: { candidates: [{ content: { parts: [{ text: "<json>" }] } }] }
        ↓
STEP 6: PARSE RESPONSE
  Extract text from candidates[0].content.parts[0].text
  Strip markdown fences if present (```json ... ```)
  Deserialise: [{ slug, reason }]
  Match each slug against the product pool
  Build AiSuggestionDto { Product: ProductSummaryDto, Reason: string }
        ↓
STEP 7: CACHE RESULT
  IMemoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(30))
        ↓
STEP 8: RETURN
  IReadOnlyList<AiSuggestionDto> → 200 OK
        ↓
ON ANY ERROR (network, parse, quota):
  Log warning
  Return middle-50% of product pool with reason "You might like this"
  If product pool itself failed: controller returns 503
```

### Why 30-Minute Cache?

The Gemini API has a per-minute and per-day quota. For an ecommerce site with hundreds of concurrent users, calling Gemini on every page load would rapidly exhaust the quota and add 500ms+ latency. The 30-minute cache means:
- The first visitor after cache expiry pays the ~500ms latency once
- All subsequent visitors within 30 minutes get instant (sub-millisecond) results
- For an anonymous user, all anonymous visitors share one cache slot
- For each authenticated user, their personalised recommendations are cached separately

---

## 5. The Checkout and Order Creation Flow

### Backend Order Creation

```
POST /api/v1/checkout
  Body: {
    deliveryAddressId: guid,
    savedCardId: guid,
    deliveryMethod: "Standard" | "Express"
  }
        ↓
CheckoutController → OrderService.PlaceOrderAsync(request, userId, ct)
        ↓
1. Load customer's basket with all items and their products
   → If basket is empty: throw validation exception → 400

2. For each basket item:
   → Create OrderItem snapshot:
      ProductName     = item.Product.Name   (SNAPSHOT)
      ProductSku      = item.Product.Sku    (SNAPSHOT)
      ProductImageUrl = item.Product.Images.FirstOrDefault()?.Url  (SNAPSHOT)
      UnitPrice       = item.UnitPrice      (SNAPSHOT — price at time of add)
      Quantity        = item.Quantity
      LineTotal       = UnitPrice * Quantity

3. Calculate SubTotal = sum of all LineTotals

4. Calculate DeliveryCharge:
   Standard delivery → FREE if SubTotal > £50.00, else £3.95
   Express delivery  → Always £5.95

5. Calculate Total = SubTotal + DeliveryCharge

6. Load delivery address → capture full address fields onto order
   (not a FK — snapshot, same principle as OrderItems)

7. Generate unique OrderNumber:
   Format: ARG + 8 random uppercase alphanumeric chars
   e.g., "ARGX7K2MNP"
   → Retry on collision (extremely rare)

8. Create Order entity:
   OrderNumber, UserId, Status=Placed, SubTotal, DeliveryCharge, Total,
   DeliveryLine1/2, DeliveryTown, DeliveryCounty, DeliveryPostcode,
   PaymentReference = savedCard.Token (or "MANUAL_ENTRY"),
   PlacedAtUtc = DateTime.UtcNow

9. Clear the basket (delete all BasketItems, reset UpdatedAtUtc)

10. Save everything in a single database transaction
    (Order + OrderItems + cleared Basket committed atomically)

11. Add initial OrderStatusHistory entry:
    Status=Placed, Note="Order placed by customer", ChangedAtUtc=now

12. Return OrderDetailDto (full detail for confirmation page)
```

**Key safety guarantees:**
- The entire operation is wrapped in a database transaction — either everything commits or nothing does
- Item prices are snapshotted from `BasketItem.UnitPrice`, which was captured when the item was added to the basket — not re-read from the product at checkout time (prevents race conditions if an admin changes a price mid-session)
- Address and product details are snapshotted so order history remains accurate after future edits

---

## 6. Order Lifecycle and Status Machine

### State Transitions

```
                    ┌──────────┐
                    │  Placed  │ (initial state, set by system at checkout)
                    └────┬─────┘
                         │ Admin confirms payment
                         ▼
                    ┌──────────┐
                    │Confirmed │
                    └────┬─────┘
                         │ Admin begins processing
                         ▼
                    ┌──────────────┐
                    │  Processing  │
                    └──────┬───────┘
                           │ Admin marks dispatched
                           ▼
                    ┌──────────────┐
                    │  Dispatched  │
                    └──────┬───────┘
                           │ Delivery confirmed
                           ▼
                    ┌──────────────┐
                    │  Delivered   │ ← TERMINAL
                    └──────────────┘

From any pre-Delivered state:
   → Cancelled (TERMINAL)
```

### How Status Changes Are Recorded

Every status change creates an `OrderStatusHistory` record:

```sql
INSERT INTO OrderStatusHistories (
    OrderId, Status, Note, ChangedAtUtc
) VALUES (
    @orderId, @newStatus, @adminNote, SYSUTCDATETIME()
)
```

The customer sees all these records as a timeline on their order detail page, showing exactly when each transition happened and any note the admin added (e.g., "Dispatched via Royal Mail, tracking: RM123456").

---

## 7. Password Reset Flow — Security Internals

### Why This Flow Is Secure

```
User submits email to /api/v1/auth/forgot-password
        ↓
System looks up user by email
        ↓
Whether or not user exists:
  → Always return 200 OK with same message
  → (Prevents attackers from discovering which emails are registered)
        ↓
If user found:
  1. Generate cryptographically random token (128-bit, URL-safe base64)
  2. Hash the token (SHA-256) — only store the hash, never the raw token
  3. Save PasswordResetToken { UserId, HashedToken, ExpiresAtUtc = now + 1h }
  4. Build reset URL: {FrontendUrl}/reset-password?token={rawToken}
  5. Send HTML email with the reset URL
        ↓
User clicks link in email
  → Angular route: /reset-password?token=<rawToken>
        ↓
POST /api/v1/auth/reset-password
  Body: { token: rawToken, newPassword: "..." }
        ↓
System:
  1. Hash the incoming raw token (SHA-256)
  2. Look up PasswordResetToken WHERE HashedToken = @hash
                                  AND ExpiresAtUtc > UTCNOW()
                                  AND UsedAtUtc IS NULL
  3. If not found: 400 Bad Request — "Invalid or expired link"
  4. If found:
     a. Hash new password with fresh salt
     b. Update user's PasswordHash
     c. Mark token as used: SET UsedAtUtc = UTCNOW()
     d. Return 200 OK
        ↓
User can now log in with new password
Old token cannot be reused (UsedAtUtc is set)
```

**Security properties:**
- Raw tokens are never stored (only their hash) — even if the database is compromised, tokens cannot be replayed
- Single-use: `UsedAtUtc` prevents replay attacks
- Time-limited: `ExpiresAtUtc` prevents tokens from being valid indefinitely
- Timing-safe: Same response whether email exists or not

---

## 8. Admin Action Audit Trail

Every mutation performed by an admin user automatically creates an `AuditLog` record.

### Record Structure

| Field | Example | Description |
|-------|---------|-------------|
| `ActorUserId` | `guid` | The admin's user ID from JWT |
| `ActorEmail` | `admin@selqi.local` | The admin's email at time of action |
| `Action` | `UpdateProduct` | The type of operation performed |
| `EntityType` | `Product` | The type of entity affected |
| `EntityId` | `guid` | The specific entity's ID |
| `Summary` | `"Updated price from £49.99 to £39.99"` | Human-readable description |
| `CreatedAtUtc` | `2026-05-14T10:23:45Z` | When the action happened |

### What Gets Logged

| Admin Action | Audit Log Entry |
|-------------|----------------|
| Create product | `Action=CreateProduct, EntityType=Product, EntityId=<new id>` |
| Update product | `Action=UpdateProduct, EntityType=Product, EntityId=<id>` |
| Create store | `Action=CreateStore, EntityType=Store` |
| Update store | `Action=UpdateStore, EntityType=Store` |
| Delete store | `Action=DeleteStore, EntityType=Store` |
| Update inventory | `Action=UpdateInventory, EntityType=Inventory` |
| Update order status | `Action=UpdateOrderStatus, EntityType=Order, Summary="Status changed to Dispatched — tracking added"` |

The audit log is **append-only**. There are no update or delete operations on this table. It grows monotonically and serves as an immutable record for compliance and investigation.

---

## 9. Caching Strategy

| Cache Subject | Cache Key | TTL | Scope | Implementation |
|--------------|-----------|-----|-------|----------------|
| AI recommendations (authenticated) | `"ai-recs:{userId}"` | 30 min sliding | Per user | `IMemoryCache` |
| AI recommendations (anonymous) | `"ai-recs:anon"` | 30 min sliding | All anonymous users | `IMemoryCache` |

**Current limitation:** `IMemoryCache` is process-local. In a horizontally scaled deployment (multiple API instances), each instance has its own independent cache. This means the first request to each instance may call Gemini independently. The solution for production scale is to replace `IMemoryCache` with a shared cache like **Redis** — the `IAiRecommendationService` interface abstracts the caching so the implementation can be swapped without changing any other code.

Categories and brands are cached **client-side** in Angular:
- `CatalogService.loadCategories()` and `loadBrands()` check a signal guard before making an HTTP call
- If the data is already loaded, no HTTP call is made
- Cache is invalidated only on page refresh or logout

---

## 10. Data Relationships Map

```
Category (hierarchical)
  ├── ParentCategoryId → Category (self-reference)
  └── Products []

Product
  ├── CategoryId → Category
  ├── BrandId → Brand
  ├── ProductImages []
  ├── ProductSpecifications []
  └── Inventory [] → Store (many-to-many via Inventory)

ApplicationUser
  ├── Basket (1:1)
  │     └── BasketItems []
  │           └── ProductId → Product
  ├── WishlistItems []
  │     └── ProductId → Product
  ├── Addresses []
  ├── SavedCards []
  ├── Orders []
  │     ├── OrderItems []         ← snapshot, no FK back to Product
  │     └── OrderStatusHistories []
  └── PasswordResetTokens []

AuditLog
  └── ActorUserId (soft reference to ApplicationUser — not FK, preserved on user deletion)
```

### Why Order Items Have No Foreign Key to Product

When a customer orders "Sony WH-1000XM5 Headphones" at £279.99, that exact name and price is captured in `OrderItems`. If the product is later renamed, repriced, or deleted, the order history still shows exactly what was bought and at what price. This is intentional and correct ecommerce behaviour.

The `ProductId` column is kept in `OrderItems` as a soft reference for analytics (e.g., "how many times was this product ordered") but it is not declared as a foreign key constraint, so it won't block product deletion.

---

## 11. Delivery Charge Calculation Logic

The delivery charge is calculated at the moment the order is placed in `OrderService`:

```csharp
private static decimal CalculateDeliveryCharge(decimal subTotal, DeliveryMethod method)
{
    return method switch
    {
        DeliveryMethod.Express  => 5.95m,
        DeliveryMethod.Standard => subTotal >= 50.00m ? 0.00m : 3.95m,
        _                       => 0.00m
    };
}
```

### Rules Summary

| Delivery Method | Subtotal < £50 | Subtotal >= £50 |
|----------------|---------------|----------------|
| Standard | £3.95 | FREE |
| Express | £5.95 | £5.95 |

The charge is captured on the `Order` record at the time of placement. If the business changes these rules in the future, historical orders are not affected — they retain the charge that was applied when they were placed.
