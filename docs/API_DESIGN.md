# API Design

The ASP.NET Core API uses JSON under `/api/v1`. Development OpenAPI metadata is exposed at `/openapi/v1.json`; `/health` is available in every environment.

## Public Catalog

- `GET /categories`, `GET /brands`
- `GET /products` with search, category, brand, price, stock, sort, and paging filters
- `GET /products/{slug}`
- `GET /products/{id}/availability`

## Authentication and Customer Resources

- `POST /auth/register`, `POST /auth/login`, `GET /auth/me`
- `POST /auth/forgot-password`, `POST /auth/reset-password`
- CRUD under `/addresses`, `/basket`, `/wishlist`, and `/saved-cards`
- `POST /checkout`
- `GET /orders`, `GET /orders/{orderNumber}`
- `GET /recommendations`

Protected requests use `Authorization: Bearer <token>`. Ownership is derived from the token, never a caller-supplied user ID.

## Payments

- `POST /razorpay/create-order` creates a sandbox order.
- `POST /razorpay/verify-payment` verifies the provider signature before checkout completion.
- Direct checkout also supports the project's local card-flow abstraction; it does not store PAN or CVV.

## Administration

`/admin` routes require the `Admin` role and provide dashboard metrics, product create/update, order status changes, audit-log queries, store CRUD, and inventory adjustments.

## Contract Conventions

- Use DTOs rather than exposing EF entities.
- Validate route, query, and body inputs before calling application services.
- Return `201 Created` for successful creation, `204 No Content` for successful deletion, `400` for invalid requests, `401/403` for authentication/authorization failures, and `404` for missing resources.
- Pass cancellation tokens through async calls and suffix asynchronous C# methods with `Async`.
