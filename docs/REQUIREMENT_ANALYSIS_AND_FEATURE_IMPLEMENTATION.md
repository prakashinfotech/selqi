# Selqi Ecommerce Portal — Requirement Analysis and Feature Implementation Document

**Audience:** Business analysts, product managers, app developers, project sponsors  
**Purpose:** Retrospective requirements capture, feature traceability, and implementation evidence  
**Version:** 1.0  
**Last Updated:** May 2026  

> **Note:** This document was produced after the initial development phase as a retrospective requirements analysis. It captures what the business needed, what was designed, and how each requirement was implemented in the delivered system.

---

## Table of Contents

1. [Project Context and Business Goals](#1-project-context-and-business-goals)
2. [Stakeholders and User Types](#2-stakeholders-and-user-types)
3. [Functional Requirements](#3-functional-requirements)
4. [Non-Functional Requirements](#4-non-functional-requirements)
5. [Feature Implementation Matrix](#5-feature-implementation-matrix)
6. [Data Requirements](#6-data-requirements)
7. [Integration Requirements](#7-integration-requirements)
8. [Compliance and Security Requirements](#8-compliance-and-security-requirements)
9. [Implementation Gaps and Backlog](#9-implementation-gaps-and-backlog)
10. [Decisions and Trade-offs Log](#10-decisions-and-trade-offs-log)

---

## 1. Project Context and Business Goals

### Background

The Selqi Ecommerce Portal is a practical full-stack demonstration of a modern retail journey from product discovery to order fulfilment, with a back-office admin portal and AI-enhanced personalisation. It began as an accelerated build and is now maintained as a working Angular, .NET, EF Core, and SQL Server application.

### Business Goals

| Goal | Priority | Status |
|------|----------|--------|
| Enable customers to browse and purchase products online | Must Have | Delivered |
| Allow customers to manage their account and order history | Must Have | Delivered |
| Provide administrators with tools to manage products and orders | Must Have | Delivered |
| Integrate AI-powered personalised recommendations | Should Have | Delivered |
| Support physical store stock visibility | Should Have | Delivered |
| Enable password self-service (forgot/reset) | Must Have | Delivered |
| Generate PDF invoices for orders | Should Have | Delivered (infrastructure only) |
| Email notifications for order events | Should Have | Partially delivered (reset email done; order email pending) |
| Wishlist for saving products | Should Have | Delivered |
| Flexible payment / finance options | Could Have | UI placeholder only |

### Scope Boundaries

**In scope (delivered):**
- Customer-facing ecommerce: Browse, search, basket, checkout, orders, wishlist
- Account management: Registration, login, addresses, saved cards, password reset
- Admin portal: Products, stores, inventory, orders, audit log
- AI recommendations: Gemini-powered, personalised, cached
- Physical store data: Locations, inventory per store
- PDF invoice service infrastructure

**Out of scope (current build):**
- Production payment settlement, refunds, webhooks, and reconciliation (Razorpay sandbox only)
- Customer-facing invoice download
- Order tracking integration (carrier APIs)
- Product reviews and ratings (display only from seed data)
- Promotional/discount code engine
- Mobile native applications
- Multi-currency or multi-language support
- Real-time stock sync with physical stores

---

## 2. Stakeholders and User Types

| Stakeholder | Role | Primary Concern |
|-------------|------|----------------|
| **Business Owner / Sponsor** | Approves features, defines priorities | ROI, feature completeness, time to market |
| **End Customer** | Browses and buys products | Ease of use, trust, fast checkout |
| **Store Staff / Admin** | Manages products and orders | Accuracy, speed, audit trail |
| **Development Team** | Builds and maintains the system | Code quality, architecture, maintainability |
| **QA / Testing** | Verifies correctness | Coverage, edge cases, regressions |

---

## 3. Functional Requirements

### FR-01: Product Catalogue

| Ref | Requirement | Acceptance Criteria |
|-----|-------------|---------------------|
| FR-01.1 | The system shall display a list of products browsable by category | Products appear filtered when a category is selected |
| FR-01.2 | The system shall support full-text search across product names | Search results reflect entered keywords |
| FR-01.3 | The system shall allow filtering by price range, brand, availability, and offers | Active filters narrow the displayed product list |
| FR-01.4 | The system shall support sorting products by rating, price, and recency | Sort order changes the displayed list order |
| FR-01.5 | The system shall paginate product results (default 24, max 100 per page) | Only the requested page of results is returned |
| FR-01.6 | Each product shall have a unique, readable URL slug | `/product/apple-airpods-pro` routes to the correct product |
| FR-01.7 | Products shall display name, images, price, was-price, rating, and brand | All fields visible on product card and detail page |
| FR-01.8 | Product detail shall include technical specifications | Spec table rendered on product detail page |
| FR-01.9 | Products shall support soft-deletion (hidden from customers, preserved in history) | Deleted products no longer appear in search; past orders retain their details |

### FR-02: Categories and Brands

| Ref | Requirement | Acceptance Criteria |
|-----|-------------|---------------------|
| FR-02.1 | Categories shall support a two-level hierarchy (parent → sub-category) | Sub-categories appear nested under their parent |
| FR-02.2 | Each category shall have a display image and ordering | Home page category strips respect DisplayOrder |
| FR-02.3 | Brands shall be listable and filterable | Brand filter on catalogue page works correctly |

### FR-03: Store Inventory

| Ref | Requirement | Acceptance Criteria |
|-----|-------------|---------------------|
| FR-03.1 | Products shall display per-store stock availability | Availability section on product detail shows store names and quantities |
| FR-03.2 | Administrators shall be able to update stock levels per product per store | Admin store inventory update endpoint saves and reflects changes |
| FR-03.3 | Stores shall have a type (Retail / Collection Point) | Store type is visible in admin and product availability |

### FR-04: Customer Account

| Ref | Requirement | Acceptance Criteria |
|-----|-------------|---------------------|
| FR-04.1 | Customers shall register with name, email, and password | Registration creates account and returns JWT |
| FR-04.2 | Customers shall log in with email and password | Login returns a valid JWT |
| FR-04.3 | Customers shall be able to reset a forgotten password via email | Reset email is sent; token-based reset completes successfully |
| FR-04.4 | Customers shall manage multiple delivery addresses | CRUD on addresses; one can be set as default |
| FR-04.5 | Customers shall save and manage payment cards (tokenised) | Card list persists; default card is honoured at checkout |

### FR-05: Shopping Basket

| Ref | Requirement | Acceptance Criteria |
|-----|-------------|---------------------|
| FR-05.1 | Authenticated customers shall have a persistent basket | Basket persists across sessions and devices |
| FR-05.2 | Customers shall add, update quantity, and remove items | Basket reflects correct state after each operation |
| FR-05.3 | Basket shall show unit prices and line totals | Prices calculated correctly |
| FR-05.4 | Basket shall apply delivery charge logic | Free standard over £50; express is £5.95 |

### FR-06: Checkout and Orders

| Ref | Requirement | Acceptance Criteria |
|-----|-------------|---------------------|
| FR-06.1 | Checkout shall require authentication | Unauthenticated users are redirected to login |
| FR-06.2 | Checkout shall capture delivery address, payment method, and delivery option | All three fields required before submission |
| FR-06.3 | Order placement shall create an order with snapshotted item details | Order items preserve name, SKU, image, and price at time of purchase |
| FR-06.4 | A unique order number shall be generated and returned | Order number displayed on confirmation page |
| FR-06.5 | Placing an order shall clear the customer's basket | Basket is empty after successful checkout |
| FR-06.6 | Customers shall view their order list and full order detail | Orders page and detail page render correctly |
| FR-06.7 | Order status shall progress through defined states | Status badge reflects current state; history timeline shows all transitions |
| FR-06.8 | Orders shall record a delivery charge and grand total | Financial summary accurate on order detail |

### FR-07: Wishlist

| Ref | Requirement | Acceptance Criteria |
|-----|-------------|---------------------|
| FR-07.1 | Authenticated customers shall save products to a wishlist | Wishlist persists and is unique per user |
| FR-07.2 | Customers shall view and remove items from the wishlist | Wishlist page renders and delete works |

### FR-08: AI Recommendations

| Ref | Requirement | Acceptance Criteria |
|-----|-------------|---------------------|
| FR-08.1 | Home page shall display 6 AI-recommended products | Recommendation section renders 6 cards |
| FR-08.2 | Recommendations shall include a personalised reason per product | Each card shows a reason string |
| FR-08.3 | Logged-in users shall receive recommendations based on order history | Gemini prompt includes purchase context |
| FR-08.4 | Anonymous users shall receive recommendations from the product pool | Default prompt used for unknown users |
| FR-08.5 | Recommendations shall be cached per user for 30 minutes | Repeated calls within 30 min return cached results |
| FR-08.6 | System shall gracefully handle Gemini unavailability | Fallback products shown; no unhandled exceptions |

### FR-09: Admin Portal

| Ref | Requirement | Acceptance Criteria |
|-----|-------------|---------------------|
| FR-09.1 | Admin portal shall require the Admin role | Non-admin users cannot access `/admin` routes |
| FR-09.2 | Dashboard shall show key business metrics | Dashboard API returns product, order, and store counts |
| FR-09.3 | Admins shall create and edit products | Product create/update API endpoints work correctly |
| FR-09.4 | Admins shall manage stores and update inventory | Store CRUD and inventory patch endpoints function correctly |
| FR-09.5 | Admins shall update order statuses with notes | Order status transitions saved; note recorded |
| FR-09.6 | All admin actions shall be logged immutably | Audit log records actor, action, entity, and timestamp |

---

## 4. Non-Functional Requirements

### NFR-01: Performance

| Ref | Requirement | Implementation |
|-----|-------------|----------------|
| NFR-01.1 | Product list page shall load within 2 seconds on typical connection | EF Core `AsNoTracking()`, paginated queries, no N+1 |
| NFR-01.2 | AI recommendations shall not block page load | Fetched asynchronously; page renders immediately |
| NFR-01.3 | Recommendation latency from Gemini shall be masked by caching | 30-min in-memory cache per user |

### NFR-02: Security

| Ref | Requirement | Implementation |
|-----|-------------|----------------|
| NFR-02.1 | Passwords shall never be stored in plain text | PBKDF2/SHA-256 with per-password salt |
| NFR-02.2 | API access to protected resources shall require a valid JWT | JWT middleware validates every authenticated request |
| NFR-02.3 | Admin-only endpoints shall enforce the Admin role | `[Authorize(Roles = "Admin")]` on all admin controllers |
| NFR-02.4 | Input shall be sanitised before database queries | EF Core parameterised queries prevent SQL injection |
| NFR-02.5 | Payment card details shall be tokenised | Only `Last4`, `Brand`, and token stored — no full card numbers |
| NFR-02.6 | Password reset tokens shall be single-use and time-limited | `UsedAtUtc` set on consumption; expiry enforced |
| NFR-02.7 | CORS shall restrict origins to configured domains | CORS policy applied from `appsettings.json` |

### NFR-03: Reliability

| Ref | Requirement | Implementation |
|-----|-------------|----------------|
| NFR-03.1 | AI service failure shall not crash the application | Try-catch with fallback product set; warning logged |
| NFR-03.2 | Order history shall survive product updates/deletions | Order items snapshot product name, SKU, image, and price |
| NFR-03.3 | Database shall be consistent (ACID) | SQL Server + EF Core transactions for checkout |

### NFR-04: Usability

| Ref | Requirement | Implementation |
|-----|-------------|----------------|
| NFR-04.1 | Every data-bound page shall handle loading, empty, error, and success states | Angular components implement all four states |
| NFR-04.2 | Filter selections shall be reflected in the URL (shareable links) | Query parameters used for all filters and pagination |
| NFR-04.3 | Images shall load lazily | `loading="lazy"` on all product image tags |

### NFR-05: Maintainability

| Ref | Requirement | Implementation |
|-----|-------------|----------------|
| NFR-05.1 | Architecture shall enforce separation of concerns | 4-layer clean architecture; each layer in its own project |
| NFR-05.2 | All async operations shall support cancellation | `CancellationToken` on all async methods |
| NFR-05.3 | Database schema changes shall be managed through migrations | EF Core migrations in `Selqi.Infrastructure` |
| NFR-05.4 | Code shall follow conventional commit standards | Commit history uses `feat:`, `fix:`, `refactor:` prefixes |

---

## 5. Feature Implementation Matrix

This matrix traces each major feature area through the layers of the system.

| Feature | Domain Entity | Application Service | API Controller | Frontend Service | Frontend Component |
|---------|--------------|--------------------|-----------------|-----------------|--------------------|
| Product catalogue | `Product`, `Category`, `Brand` | `CatalogService` | `CatalogController` | `CatalogService` | `catalog/`, `product/` |
| Store inventory | `Store`, `Inventory` | `CatalogService`, `AdminService` | `CatalogController`, `AdminController` | `CatalogService`, `AdminService` | `product/` availability, `admin/stores/` |
| User auth | `ApplicationUser` | `AuthService` | `AuthController` | `AuthService` | `auth/login/`, `auth/register/` |
| Addresses | `Address` | `AddressService` | `AddressController` | `AddressService` | `checkout/`, account section |
| Saved cards | `SavedCard` | `SavedCardService` | `SavedCardsController` | `SavedCardService` | `checkout/`, account section |
| Password reset | `PasswordResetToken` | `AuthService` | `AuthController` | `AuthService` | `auth/forgot-password/`, `auth/reset-password/` |
| Basket | `Basket`, `BasketItem` | `BasketService` | `BasketController` | `BasketService` | `basket/` |
| Checkout | `Order`, `OrderItem` | `OrderService` | `CheckoutController` | `OrderService` | `checkout/` |
| Order history | `Order`, `OrderItem`, `OrderStatusHistory` | `OrderService` | `OrdersController` | `OrderService` | `orders/order-list/`, `orders/confirmation/` |
| Wishlist | `WishlistItem` | `WishlistService` | `WishlistController` | `WishlistService` | `wishlist/` |
| AI recommendations | *(no domain entity)* | `IAiRecommendationService` | `RecommendationsController` | `RecommendationService` | `home/` AI section |
| Admin dashboard | `AuditLog`, Orders, Products | `AdminService` | `AdminController` | `AdminService` | `admin/admin-dashboard` |
| Admin products | `Product` | `AdminService` | `AdminController` | `AdminService` | `admin/products/` |
| Admin stores | `Store`, `Inventory` | `AdminService` | `AdminController` | `AdminService` | `admin/stores/` |
| Admin orders | `Order`, `OrderStatusHistory` | `AdminService` | `AdminController` | `AdminService` | `admin/orders/` |
| Audit log | `AuditLog` | `AdminService` | `AdminController` | `AdminService` | `admin/audit/` |
| Email notifications | — | `IEmailService` | *(called internally)* | — | — |
| PDF invoices | — | `IInvoicePdfService` | *(available for wiring)* | — | — |

---

## 6. Data Requirements

### Master Data

| Data Set | Source | Owner | Update Frequency |
|----------|--------|-------|-----------------|
| Product catalogue | Seed data (`CatalogSeedData.cs`) | Admin | On demand |
| Categories | Seed data | Admin | Rarely |
| Brands | Seed data | Admin | Rarely |
| Store locations | Seed data | Admin | Rarely |
| Initial inventory | Seed data | Admin | Daily/as needed |

### Transactional Data

| Data Set | Created By | Retention |
|----------|-----------|-----------|
| User accounts | Self-registration or admin | Indefinite |
| Baskets | System (per user) | Until cleared or ordered |
| Orders and items | Checkout | Indefinite (audit) |
| Order status histories | Admin actions | Indefinite (audit) |
| Audit logs | Admin actions | Indefinite |
| Password reset tokens | Forgot-password flow | Auto-expiry |

### Data Integrity Rules

- Products can only be soft-deleted — hard deletion is not permitted if any order references the product
- Order items must snapshot `ProductName`, `ProductSku`, `ProductImageUrl`, and `UnitPrice` at checkout time
- Inventory records must have a unique `(ProductId, StoreId)` pair
- Wishlist items must be unique per `(UserId, ProductId)` pair
- Money values stored as `decimal(18,2)` — never as `float` or `double`
- All timestamps stored in UTC

---

## 7. Integration Requirements

### Google Gemini AI

| Aspect | Detail |
|--------|--------|
| Purpose | Product recommendation generation |
| Endpoint | `https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent` |
| Authentication | API key via `Gemini:ApiKey` configuration |
| Model | `gemini-1.5-flash` (configurable) |
| Dependency level | Optional — system functions without it (fallback mode) |
| Caching | 30-minute in-memory cache to minimise API calls |

### SMTP Email (Office 365)

| Aspect | Detail |
|--------|--------|
| Purpose | Password reset emails |
| Host | `smtp.office365.com:587` |
| Transport | STARTTLS |
| Dependency level | Optional — auth functions without email; reset flow is degraded |

### Future Integration Points (Planned)

| Integration | Purpose | Status |
|-------------|---------|--------|
| Razorpay | Sandbox payment order creation and server-side signature verification | Implemented when sandbox credentials are configured; production settlement and reconciliation remain out of scope |
| Carrier tracking API | Live dispatch/delivery updates | Not implemented |
| Customer email notifications | Order confirmation, dispatch alerts | Email infrastructure in place; templates pending |

---

## 8. Compliance and Security Requirements

### CR-01: Data Protection

| Ref | Requirement | Implementation Status |
|-----|-------------|----------------------|
| CR-01.1 | No full payment card numbers shall be stored | Delivered — only `Last4`, `Brand`, `Token` stored |
| CR-01.2 | Passwords shall be stored using a one-way hash with salt | Delivered — PBKDF2/SHA-256 |
| CR-01.3 | Password reset links shall expire and be single-use | Delivered — `ExpiresAtUtc` and `UsedAtUtc` enforced |
| CR-01.4 | JWT tokens shall be signed and validated | Delivered — HMAC-SHA256 signing |

### CR-02: Access Control

| Ref | Requirement | Implementation Status |
|-----|-------------|----------------------|
| CR-02.1 | Customer data shall only be accessible by the owning customer | Delivered — user ID from JWT claim checked on all user-scoped queries |
| CR-02.2 | Admin endpoints shall require Admin role | Delivered — `[Authorize(Roles = "Admin")]` |
| CR-02.3 | All admin actions shall be audited | Delivered — `AuditLog` table, immutable |

### CR-03: API Security

| Ref | Requirement | Implementation Status |
|-----|-------------|----------------------|
| CR-03.1 | API shall reject requests from unauthorised origins | Delivered — CORS policy enforced |
| CR-03.2 | API shall use HTTPS in production | Infrastructure responsibility — HTTPS configured in `appsettings.json` |
| CR-03.3 | API keys shall not be committed to source control | Delivered — API key in `appsettings.Development.json` (git-ignored) |

---

## 9. Implementation Gaps and Backlog

The following items were identified as requirements but were not fully implemented in the current build. These form the basis of the next development cycle.

### High Priority Backlog

| Item | Category | Effort Estimate | Dependency |
|------|----------|----------------|------------|
| Order confirmation email | Email | Small | `SmtpEmailService` (done) — needs template + trigger |
| Dispatch notification email | Email | Small | Same as above |
| PDF invoice download for customers | Feature | Medium | `QuestPdfInvoiceService` (done) — needs endpoint + UI |
| Production payment rollout | Integration | Large | Provider onboarding, webhooks, refunds, reconciliation, and PCI/security review |

### Medium Priority Backlog

| Item | Category | Effort Estimate |
|------|----------|----------------|
| Product reviews and ratings | Feature | Medium |
| Promotional/discount code engine | Feature | Large |
| Customer-facing store finder map | UI | Medium |
| Real-time stock sync with stores | Integration | Large |
| Advanced admin search and filtering | Admin | Small |

### Low Priority / Future Roadmap

| Item | Category |
|------|----------|
| Mobile app (iOS / Android) | Platform |
| Multi-currency support | Internationalisation |
| Multi-language / localisation | Internationalisation |
| A/B testing framework | Experimentation |
| Enhanced analytics dashboard | Admin |

---

## 10. Decisions and Trade-offs Log

This log records key architectural and feature decisions made during the build, including the rationale and the trade-off accepted.

| Decision | Rationale | Trade-off Accepted |
|----------|-----------|-------------------|
| **Clean 4-layer architecture** | Enforces separation of concerns; easy to test each layer independently | More boilerplate than a monolithic approach |
| **JWT for authentication** | Stateless; no session storage needed on server | Tokens cannot be individually revoked before expiry (24h TTL mitigates this) |
| **Soft delete on products** | Preserves referential integrity in historical orders | Deleted products accumulate in DB; need periodic archival strategy |
| **Order item snapshots** | Guarantees order accuracy even after product edits | Data duplication (product name/price stored twice) |
| **Gemini over other AI providers** | Free tier available; fast `gemini-1.5-flash` model; simple REST API | Vendor lock-in to Google AI; if API changes, service needs updating |
| **Angular Signals over RxJS** | Simpler mental model; less boilerplate; future-proof with Angular 21+ | Less ecosystem tooling compared to RxJS-based patterns |
| **No real payment processing** | Avoids PCI compliance complexity in the prototype | Checkout flow is not production-ready without a real gateway |
| **IMemoryCache for AI results** | Zero infrastructure dependencies; instant setup | Cache is process-local — lost on app restart; not shared across instances |
| **QuestPDF for invoices** | Pure .NET; no external service dependency; good layout control | PDF layout requires code changes (not template-driven) |
| **Admin seed via HostedService** | Admin account is always available on startup without manual DB steps | Seed credentials must be secured in production config |
| **Office 365 SMTP for email** | Reuses existing business email infrastructure | Rate-limited by Microsoft; dedicated transactional email service preferred for production |
