# Selqi Ecommerce Portal — Technical Documentation

**Audience:** Software developers, technical architects, engineering managers, business heads with technical oversight  
**Version:** 1.0  
**Last Updated:** May 2026  

---

## Table of Contents

1. [Executive Summary](#1-executive-summary)
2. [Technology Stack](#2-technology-stack)
3. [System Architecture](#3-system-architecture)
4. [Backend Layer Details](#4-backend-layer-details)
5. [Frontend Architecture](#5-frontend-architecture)
6. [Database Design](#6-database-design)
7. [API Reference](#7-api-reference)
8. [Authentication and Security](#8-authentication-and-security)
9. [AI Recommendation Engine](#9-ai-recommendation-engine)
10. [Email and Notification System](#10-email-and-notification-system)
11. [PDF Invoice Generation](#11-pdf-invoice-generation)
12. [Deployment and Infrastructure](#12-deployment-and-infrastructure)
13. [Performance Considerations](#13-performance-considerations)
14. [Coding Conventions and Patterns](#14-coding-conventions-and-patterns)

---

## 1. Executive Summary

The Selqi Ecommerce Portal is a full-stack, production-grade ecommerce web application modelled after the Selqi retail experience. It combines a modern Angular 21 frontend with an ASP.NET Core .NET 10 backend, persisted in SQL Server, and enhanced with Google Gemini AI-powered product recommendations.

The system supports the complete customer lifecycle — product discovery, account management, basket, checkout, order tracking, and wishlisting — alongside a secure administrative back-office for inventory, orders, and audit logging.

**Key Metrics:**
- ~20 database tables, fully normalised to 3NF
- 10+ REST API controllers covering ~40 endpoints
- 8+ feature modules in the frontend
- AI recommendation engine with 30-minute caching
- Layered clean architecture (Domain → Application → Infrastructure → API)

---

## 2. Technology Stack

### Backend

| Concern | Technology | Version |
|---------|-----------|---------|
| Runtime | .NET | 10.0 |
| Web framework | ASP.NET Core Web API | 10.0 |
| ORM | Entity Framework Core | 9.x |
| Database | SQL Server | 2019+ |
| Authentication | JWT Bearer Tokens | Microsoft.AspNetCore.Authentication.JwtBearer |
| AI / LLM | Google Gemini REST API | gemini-1.5-flash |
| Email | SMTP (Office 365) | MailKit / SmtpClient |
| PDF generation | QuestPDF | latest |
| In-memory cache | IMemoryCache | Built-in |
| HTTP client | IHttpClientFactory | Built-in |
| OpenAPI | Built-in ASP.NET Core OpenAPI | — |

### Frontend

| Concern | Technology | Version |
|---------|-----------|---------|
| Framework | Angular | 21.x |
| Language | TypeScript | 5.x |
| Styling | SCSS | — |
| State | Angular Signals | — |
| HTTP | Angular HttpClient | — |
| Build | Angular CLI / Vite | — |
| Unit testing | Vitest | — |
| E2E testing | Playwright | — |
| Package manager | npm | 11.12.1 |

### Infrastructure

| Concern | Technology |
|---------|-----------|
| Database host | SQL Server Express (local dev) / SQL Server (prod) |
| Auth | JWT (1440-minute expiry, configurable) |
| Secrets | appsettings.json / environment variables |
| CORS | Configured per environment |

---

## 3. System Architecture

### High-Level Overview

```
┌─────────────────────────────────────────────────────┐
│                  Angular 21 SPA                     │
│  (localhost:4200 / production domain)               │
│                                                     │
│  Features: Home, Catalog, Product, Basket,          │
│            Checkout, Orders, Wishlist, Auth, Admin  │
└──────────────────────┬──────────────────────────────┘
                       │ HTTPS / REST JSON
                       │ Authorization: Bearer <JWT>
┌──────────────────────▼──────────────────────────────┐
│              ASP.NET Core 10 Web API                │
│              (localhost:5123 / :7189)               │
│                                                     │
│  Controllers: Catalog, Auth, Basket, Orders,        │
│               Checkout, Addresses, SavedCards,      │
│               Wishlist, Admin, Recommendations      │
└──────┬───────────────┬──────────────────────────────┘
       │               │
┌──────▼──────┐  ┌─────▼──────────────────────────────┐
│  SQL Server │  │  Google Gemini REST API             │
│  (SelqiDb)  │  │  generativelanguage.googleapis.com  │
│             │  │  (AI recommendations)               │
│  18 tables  │  └────────────────────────────────────┘
└─────────────┘
```

### Clean Architecture Layers

The backend follows strict 4-layer clean architecture. Dependencies flow inward only.

```
┌────────────────────────────────────────────────────────────────┐
│  API Layer (Selqi.Api)                                         │
│  • Thin controllers — validate, delegate, return HTTP response │
│  • Program.cs, middleware, OpenAPI                             │
├────────────────────────────────────────────────────────────────┤
│  Infrastructure Layer (Selqi.Infrastructure)                   │
│  • EF Core DbContext + repositories                            │
│  • JWT token service, email, PDF, Gemini AI                    │
│  • Migrations, seed data                                       │
├────────────────────────────────────────────────────────────────┤
│  Application Layer (Selqi.Application)                         │
│  • Service interfaces + implementations                        │
│  • DTOs (all API-facing models)                                │
│  • Repository interfaces                                       │
├────────────────────────────────────────────────────────────────┤
│  Domain Layer (Selqi.Domain)                                   │
│  • Pure entities — zero framework dependencies                 │
│  • Enums, value objects                                        │
└────────────────────────────────────────────────────────────────┘
```

**Dependency rule:** Each layer depends only on the layer below it. The Domain layer has no dependencies at all.

### Request Lifecycle

```
HTTP Request
    ↓
ASP.NET Core Middleware Pipeline
    (CORS → Authentication → Authorization → Routing)
    ↓
Controller Action
    ↓  (delegates to)
Application Service
    ↓  (calls)
Repository Interface  ←→  EF Core Implementation  ←→  SQL Server
    ↓  (returns DTO)
Controller
    ↓
HTTP Response (JSON)
```

---

## 4. Backend Layer Details

### 4.1 Domain Layer (`Selqi.Domain`)

Pure C# classes representing business entities. No EF Core attributes, no framework references.

| Entity | Key Fields | Relationships |
|--------|-----------|--------------|
| `Product` | Id, Name, Slug, Sku, Price, WasPrice, Rating, ReviewCount, IsFeatured, IsDeleted | → Category, Brand, ProductImages, Specifications, Inventory |
| `Category` | Id, Name, Slug, ParentCategoryId, ImageUrl, DisplayOrder | Self-referencing hierarchy |
| `Brand` | Id, Name, Slug | — |
| `Store` | Id, Name, Address, StoreType, Lat/Lng, IsActive | → Inventory |
| `Inventory` | ProductId, StoreId, QuantityAvailable, LowStockThreshold | Junction table |
| `ApplicationUser` | Id, Email, PasswordHash, FirstName, LastName, Role | → Basket, Orders, Addresses, Cards |
| `Order` | Id, OrderNumber, UserId, Status, SubTotal, DeliveryCharge, Total | → OrderItems, StatusHistories |
| `Basket` | Id, UserId | → BasketItems |
| `WishlistItem` | UserId, ProductId | — |

### 4.2 Application Layer (`Selqi.Application`)

**Service registration** (`DependencyInjection.cs`): All services registered as `Scoped`.

| Service | Interface | Responsibility |
|---------|-----------|----------------|
| `CatalogService` | `ICatalogService` | Product/category/brand read queries |
| `AuthService` | `IAuthService` | Registration, login, JWT generation |
| `BasketService` | `IBasketService` | Cart CRUD, price calculation |
| `OrderService` | `IOrderService` | Checkout, order history |
| `AddressService` | `IAddressService` | Delivery address management |
| `SavedCardService` | `ISavedCardService` | Tokenized card storage |
| `WishlistService` | `IWishlistService` | Saved products list |
| `AdminService` | `IAdminService` | Product management, order status, audit |

**DTO conventions:**
- Input models: `XxxRequest` (e.g., `RegisterRequest`, `CheckoutRequest`)
- Output models: `XxxDto` (e.g., `ProductDetailDto`, `OrderSummaryDto`)
- Paged results: `PagedResultDto<T>` — always includes `items`, `page`, `pageSize`, `totalItems`, `totalPages`

### 4.3 Infrastructure Layer (`Selqi.Infrastructure`)

**SelqiDbContext** configures 18 application entity tables with:
- Decimal precision: `decimal(18,2)` for all money fields
- UTC timestamps: `SYSUTCDATETIME()` SQL defaults
- Unique indexes: Slug, SKU, Email, OrderNumber, Token
- Composite unique: `(ProductId, StoreId)` on Inventory; `(UserId, ProductId)` on WishlistItems
- Soft delete: `IsDeleted` filter on Products and Categories

**Repository implementations** use only LINQ — no raw SQL. All methods are async with `CancellationToken`.

**Key infrastructure services:**
- `JwtTokenService`: Generates signed JWT with claims (`sub`, `email`, `given_name`, `family_name`, `role`)
- `SmtpEmailService`: Connects to Office 365 SMTP, sends HTML emails
- `QuestPdfInvoiceService`: Generates styled PDF invoices for orders
- `GeminiRecommendationService`: Queries Gemini AI for personalized product recommendations
- `AdminSeedService`: Hosted service that seeds the admin account on startup

### 4.4 API Layer (`Selqi.Api`)

**`Program.cs` configures:**
- JWT authentication with symmetric key validation
- CORS policy allowing configured origins
- `AddApplicationServices()` + `AddInfrastructureServices(config)` DI registration
- Health check at `/health`
- OpenAPI documentation at `/openapi/v1.json` (dev only)
- Auto-migration on Docker/container startup

**Controller pattern:**
```csharp
// Thin — no business logic here
[HttpGet("{slug}")]
[ProducesResponseType<ProductDetailDto>(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<IActionResult> GetProduct(string slug, CancellationToken ct)
{
    var product = await _catalogService.GetProductBySlugAsync(slug, ct);
    return product is null ? NotFound() : Ok(product);
}
```

---

## 5. Frontend Architecture

### 5.1 Folder Structure

```
src/app/
├── features/          ← Page-level feature modules
│   ├── home/          ← Landing page with AI picks
│   ├── catalog/       ← Product grid with filters
│   ├── product/       ← Product detail + availability
│   ├── basket/        ← Shopping cart
│   ├── checkout/      ← Checkout flow
│   ├── orders/        ← Order list + confirmation
│   ├── wishlist/      ← Saved items
│   ├── auth/          ← Login, register, password reset
│   └── admin/         ← Admin back-office
├── core/
│   ├── services/      ← Singleton HTTP services
│   ├── guards/        ← Route guards (auth, admin)
│   └── interceptors/  ← JWT injection + 401 handling
├── shared/
│   ├── components/    ← Header, footer, reusable UI
│   └── models/        ← TypeScript interfaces
├── app.routes.ts      ← Centralised routing
└── app.config.ts      ← Root DI/providers config
```

### 5.2 State Management with Angular Signals

Components use `signal()` for local state and `computed()` for derived state. No external state library is used.

```typescript
// Example from AuthService
private _user = signal<UserDto | null>(null);
readonly currentUser = this._user.asReadonly();
readonly isAuthenticated = computed(() => this._user() !== null);
readonly isAdmin = computed(() => this._user()?.role === 'Admin');
```

### 5.3 HTTP Services

All services return `Observable<T>` using Angular's `HttpClient`. Services are provided in `root` (singleton).

```typescript
getSuggestions(): Observable<AiSuggestion[]> {
    return this.http.get<AiSuggestion[]>(`${this.apiBase}/recommendations`);
}
```

### 5.4 Authentication Flow (Frontend)

1. User submits login form → `AuthService.login()` called
2. API returns `{ token, expiresAt, user }`
3. Token stored in `localStorage` under key `selqi_token`
4. User object stored under key `selqi_user`
5. `AuthInterceptor` attaches `Authorization: Bearer <token>` to every outgoing request
6. On 401 response: `AuthService.logout()` clears storage and redirects to `/login`

### 5.5 Routing and Guards

| Route | Guard | Notes |
|-------|-------|-------|
| `/basket`, `/checkout` | `AuthGuard` | Redirects to `/login` |
| `/orders`, `/wishlist` | `AuthGuard` | Redirects to `/login` |
| `/admin/**` | `AuthGuard` + `AdminGuard` | Requires `role === 'Admin'` |
| `/products`, `/product/:slug` | None | Public |
| `/login`, `/register` | None | Public |

---

## 6. Database Design

### Entity Relationship Summary

```
Category ─────────── Product ──────── Brand
    │                   │
    │         ProductImages, ProductSpecifications
    │                   │
    │               Inventory ────── Store
    │
ApplicationUser ─── Basket ──── BasketItem ──── Product
        │
        ├── Order ──── OrderItem (snapshot)
        │       └── OrderStatusHistory
        │
        ├── Address
        ├── SavedCard
        ├── WishlistItem ──── Product
        └── PasswordResetToken
```

### Table Reference

| Table | Purpose | Row Estimate |
|-------|---------|-------------|
| `Categories` | Product classification hierarchy | ~30 |
| `Brands` | Product manufacturers | ~20 |
| `Products` | Core product catalog | ~100+ |
| `ProductImages` | Image URLs per product | ~300+ |
| `ProductSpecifications` | Technical specs | ~500+ |
| `Stores` | Physical store locations | ~10 |
| `Inventory` | Stock levels per product per store | ~1000+ |
| `Users` | Customer and admin accounts | Varies |
| `Addresses` | Delivery addresses | Varies |
| `SavedCards` | Tokenized payment methods | Varies |
| `Baskets` | One basket per user | = Users |
| `BasketItems` | Cart contents | Varies |
| `WishlistItems` | Saved products | Varies |
| `Orders` | Order master records | Varies |
| `OrderItems` | Snapshot of items at time of purchase | Varies |
| `OrderStatusHistories` | Full status audit trail | Varies |
| `PasswordResetTokens` | Secure reset links | Varies |
| `AuditLogs` | Admin operations log | Grows over time |

### Key Design Decisions

- **Slug-based URLs** — every product, category, and brand has a unique URL-safe slug, enabling SEO-friendly routes
- **Order snapshots** — OrderItems capture `ProductName`, `ProductSku`, `UnitPrice`, and `ProductImageUrl` at checkout time, so orders remain accurate even if the product is later updated or deleted
- **Soft deletes** — Products and categories use `IsDeleted` rather than hard deletion, preserving order history integrity
- **UTC everywhere** — All timestamps stored in UTC; conversion to local time happens in the UI
- **Composite unique constraint** on `(ProductId, StoreId)` in Inventory prevents duplicate stock records
- **Delivery charge logic** — Free for standard delivery over £50; express costs £5.95 regardless

---

## 7. API Reference

**Base URL:** `http://localhost:5123/api/v1` (development)  
**Auth header:** `Authorization: Bearer <JWT>`  
**Content-Type:** `application/json`

### Catalog (Public)

| Method | Path | Description |
|--------|------|-------------|
| GET | `/categories` | All categories with sub-categories |
| GET | `/brands` | All brands |
| GET | `/products` | Paged product list with optional filters |
| GET | `/products/{slug}` | Single product detail |
| GET | `/products/{id}/availability` | Stock levels by store |

**Product query parameters:** `search`, `category`, `brand`, `minPrice`, `maxPrice`, `availableOnly`, `offersOnly`, `sort` (rating/price-asc/price-desc/newest), `page`, `pageSize`

### Authentication (Public)

| Method | Path | Description |
|--------|------|-------------|
| POST | `/auth/register` | Create account, returns JWT |
| POST | `/auth/login` | Login, returns JWT |
| GET | `/auth/me` | Get current user (requires auth) |
| POST | `/auth/forgot-password` | Send reset email |
| POST | `/auth/reset-password` | Submit new password with token |

### Basket (Authenticated)

| Method | Path | Description |
|--------|------|-------------|
| GET | `/basket` | Get current basket |
| POST | `/basket/items` | Add item to basket |
| PUT | `/basket/items/{itemId}` | Update item quantity |
| DELETE | `/basket/items/{itemId}` | Remove item |
| DELETE | `/basket` | Clear entire basket |

### Orders and Checkout (Authenticated)

| Method | Path | Description |
|--------|------|-------------|
| POST | `/checkout` | Place order from basket |
| GET | `/orders` | List all user orders |
| GET | `/orders/{orderNumber}` | Order detail |

### Account (Authenticated)

| Method | Path | Description |
|--------|------|-------------|
| GET | `/addresses` | List saved addresses |
| POST | `/addresses` | Add address |
| PUT | `/addresses/{id}` | Update address |
| DELETE | `/addresses/{id}` | Delete address |
| PUT | `/addresses/{id}/default` | Set as default |
| GET | `/saved-cards` | List saved cards |
| POST | `/saved-cards` | Save a card |
| DELETE | `/saved-cards/{id}` | Remove card |
| PUT | `/saved-cards/{id}/default` | Set as default |
| GET | `/wishlist` | Get wishlist |
| POST | `/wishlist/items` | Add to wishlist |
| DELETE | `/wishlist/items/{productId}` | Remove from wishlist |

### Admin (Authenticated + Admin Role)

| Method | Path | Description |
|--------|------|-------------|
| GET | `/admin/dashboard` | Summary metrics |
| GET | `/admin/products` | All products for management |
| POST | `/admin/products` | Create product |
| PUT | `/admin/products/{id}` | Update product |
| GET | `/admin/orders` | All orders |
| PATCH | `/admin/orders/{id}/status` | Update order status |
| GET | `/admin/audit-logs` | Audit trail |
| GET | `/admin/stores` | All stores |
| POST | `/admin/stores` | Create store |
| PUT | `/admin/stores/{id}` | Update store |
| PATCH | `/admin/stores/{id}/inventory` | Update stock level |
| DELETE | `/admin/stores/{id}` | Remove store |

### Recommendations (Public)

| Method | Path | Description |
|--------|------|-------------|
| GET | `/recommendations` | AI-powered product suggestions |

**Response:** `200 OK` with `[{ product: ProductSummaryDto, reason: string }]` or `503 Service Unavailable` if Gemini is unreachable.

---

## 8. Authentication and Security

### JWT Token Structure

```json
{
  "sub": "<userId>",
  "email": "user@example.com",
  "given_name": "John",
  "family_name": "Doe",
  "role": "Customer",
  "exp": 1234567890
}
```

- Signed with HMAC-SHA256 (symmetric key from config)
- Expiry: 1440 minutes (24 hours) by default
- Claims are validated on every request

### Password Security

- Passwords hashed using `PBKDF2` with SHA-256
- Salt is generated per password; stored with hash
- Reset tokens are single-use, time-limited, and stored hashed

### Role Model

| Role | Access Level |
|------|-------------|
| `Customer` | Own basket, orders, wishlist, addresses, cards |
| `Admin` | Everything a Customer can do + full admin panel |

### CORS Policy

Allowed origins are configured in `appsettings.json` under `Cors:AllowedOrigins`. In development: `http://localhost:4200` and `https://localhost:4200`. In production: the deployed frontend domain.

### Input Validation

- All string inputs normalised (lowercase + trim) before database queries
- Model validation via ASP.NET Core `[Required]`, `[EmailAddress]`, `[MinLength]` attributes
- SQL injection prevented entirely by EF Core parameterised queries
- XSS prevented by JSON serialisation (never render raw HTML from API)

---

## 9. AI Recommendation Engine

### Overview

The recommendation engine uses the **Google Gemini API** to generate personalised product suggestions displayed on the home page.

### Request Flow

```
Frontend Request: GET /api/v1/recommendations
        ↓
RecommendationsController → GeminiRecommendationService
        ↓
1. Check IMemoryCache (key: "ai-recs:{userId|anon}")
   → Cache hit: return immediately (30-min TTL)
   → Cache miss: continue
        ↓
2. Load top 50 newest in-stock products from DB
        ↓
3. Build user context:
   → Authenticated: "This customer has previously purchased: {last 5 order items}"
   → Anonymous: "This is a new visitor with no purchase history."
        ↓
4. POST to Gemini API:
   URL: https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent
   Body: { contents: [{ parts: [{ text: <prompt> }] }] }
        ↓
5. Parse JSON response: [{ "slug": "...", "reason": "..." }]
        ↓
6. Match slugs against product pool → build AiSuggestionDto list
        ↓
7. Store in cache for 30 minutes
        ↓
8. Return 6 recommendations with reasons
```

### Prompt Design

The system prompt instructs Gemini to:
- Return exactly 6 products from the provided pool
- Vary recommendations across different categories
- Write short, human-readable reason strings (1–2 sentences)
- Respond in pure JSON without markdown fences

### Fallback Behaviour

If Gemini is unreachable or returns an error:
- Service logs a warning
- Returns the middle 6 products from the pool with reason `"You might like this"`
- Controller returns `503 Service Unavailable` only if no fallback is possible
- The frontend displays an "AI suggestions unavailable right now" message gracefully

### Configuration

```json
{
  "Gemini": {
    "ApiKey": "<your-api-key>",
    "Model": "gemini-1.5-flash"
  }
}
```

---

## 10. Email and Notification System

**Provider:** Office 365 SMTP (`smtp.office365.com:587`)  
**Transport:** STARTTLS

**Triggered by:**

| Trigger | Email Sent |
|---------|-----------|
| User requests password reset | HTML email with secure reset link |

**Configuration (`appsettings.json`):**
```json
{
  "Email": {
    "SenderEmail": "notification@example.com",
    "Host": "smtp.office365.com",
    "Port": 587,
    "Username": "...",
    "Password": "...",
    "FrontendUrl": "http://localhost:4200"
  }
}
```

Reset links expire after a configurable duration and are single-use (marked `UsedAtUtc` on consumption).

---

## 11. PDF Invoice Generation

**Library:** QuestPDF  
**Service:** `IInvoicePdfService` / `QuestPdfInvoiceService` (registered as Singleton)

Generates a styled invoice PDF including:
- Order number, date, and status
- Customer and delivery details
- Itemised product list with unit prices and line totals
- Subtotal, delivery charge, and grand total
- Company branding

Invoked from the Orders flow after a successful checkout.

---

## 12. Deployment and Infrastructure

### Environment Variables / Configuration

| Key | Description |
|-----|-------------|
| `ConnectionStrings__SelqiDb` | SQL Server connection string |
| `Jwt__Secret` | HMAC signing key (min 32 chars) |
| `Jwt__Issuer` | Token issuer (e.g., "SelqiApi") |
| `Jwt__Audience` | Token audience (e.g., "SelqiClient") |
| `Jwt__ExpiryMinutes` | Token lifetime |
| `Email__SenderEmail` | SMTP sender |
| `Email__Host` | SMTP host |
| `Email__Port` | SMTP port |
| `Email__SenderEmail` | SMTP sender and login address |
| `Email__Password` | SMTP credentials |
| `Gemini__ApiKey` | Google Gemini API key |
| `Gemini__Model` | Model ID (e.g., gemini-1.5-flash) |
| `Cors__AllowedOrigins__0` | Frontend origin |

### Database Migrations

```powershell
dotnet tool restore
dotnet tool run dotnet-ef migrations add <MigrationName> --project src/backend/Selqi.Infrastructure --startup-project src/backend/Selqi.Api
dotnet tool run dotnet-ef database update --project src/backend/Selqi.Infrastructure --startup-project src/backend/Selqi.Api
dotnet tool run dotnet-ef migrations script --idempotent --project src/backend/Selqi.Infrastructure --startup-project src/backend/Selqi.Api --output database/migrations/001_complete_schema_and_seed.sql
```

### Docker Notes

`Program.cs` auto-applies pending migrations only when `ASPNETCORE_ENVIRONMENT` is `Docker`. Other environments should apply the reviewed idempotent SQL script as a controlled deployment step; automatic migration alone does not guarantee zero-downtime upgrades.

### Health Check

`GET /health` returns `200 OK` with body `Healthy` when the service is running. Use this for load balancer and monitoring probes.

---

## 13. Performance Considerations

| Area | Approach |
|------|---------|
| AI Recommendations | 30-minute in-memory cache per user (or anonymous) |
| Category list | Loaded once per client session via Angular signal guard |
| Product images | Lazy-loaded in the browser (`loading="lazy"`) |
| Paged queries | Max page size of 100 enforced server-side |
| EF Core queries | `AsNoTracking()` on all read-only repository methods |
| HTTP client | `IHttpClientFactory` manages connection pooling for Gemini calls |

---

## 14. Coding Conventions and Patterns

### Backend

- All service and repository methods are `async Task<T>` with `CancellationToken ct` parameter
- DTOs are immutable records where possible
- Controllers never contain business logic — they validate input and delegate to services
- Repository methods contain LINQ; services contain orchestration logic
- `PagedResultDto<T>` is the standard shape for any list endpoint

### Frontend

- Strict TypeScript compiler options: `noImplicitOverride`, `strictTemplates`, `noImplicitReturns`
- Angular Signals for all reactive state — no RxJS `BehaviorSubject` in components
- Every data-bound page handles four states: `loading`, `empty`, `error`, `success`
- Query parameters (not route params) for filters and pagination — keeps URLs bookmarkable
- `track` functions on all `@for` loops for optimal DOM diffing
- Component styles in their own `.scss` file; no inline styles
