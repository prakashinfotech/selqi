# Current Application Requirements

## Customer Experience

- Browse category and product pages, search the catalog, filter/sort results, and view store availability.
- Register, sign in, recover a password, and access protected account features with JWT authentication.
- Manage a basket, wishlist, delivery addresses, and saved-card display metadata.
- Select delivery or collection, complete checkout through the local card flow or Razorpay sandbox, and view order history/status.
- Receive product recommendations using the Gemini integration when configured, with application fallback behavior when it is unavailable.

## Administration

- Require the `Admin` role for the administration area.
- Display dashboard metrics and paginated operational data.
- Create/update products, manage stores and inventory, update order status, and record audit events.

## Engineering Requirements

- Use Angular 21, .NET 10, EF Core 10, and SQL Server 2022.
- Preserve dependency direction from API/Infrastructure toward Application and Domain.
- Version HTTP routes under `/api/v1`; use DTOs and async cancellation-aware services.
- Keep schema changes reproducible through EF migrations and the checked-in idempotent SQL script.
- Keep all real credentials in User Secrets, environment variables, or a deployment secret store.
- Pass Release builds, xUnit tests, frontend unit tests, and relevant Playwright scenarios.

## Current Boundaries

This repository is a demonstration application, not a production commerce platform. Razorpay must use sandbox credentials for local testing. Shipping/carrier integrations, production tax rules, refunds, fraud tooling, customer notification workflows, and formal PCI/privacy certification are outside the current implementation. Do not use real cardholder or customer data.
