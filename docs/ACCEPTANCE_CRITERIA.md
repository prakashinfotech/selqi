# Acceptance Criteria

## Setup and Operations

- A fresh clone can be configured from `.env.example` without editing tracked secrets.
- `docker compose up --build` starts healthy SQL Server, API, and web services and applies all migrations.
- Manual setup works with .NET 10, Node.js 22/npm 11, and SQL Server 2022/Express.
- `/health` returns success and the Angular application can call `/api/v1`.

## Customer Capabilities

- Catalog browsing, search, filters, product details, and store availability return seeded data.
- A customer can register/login and manage owned basket, wishlist, address, and saved-card metadata.
- Checkout validates stock and address/fulfilment input, creates an order, clears purchased basket items, and exposes order history.
- Razorpay sandbox completion requires successful server-side signature verification.

## Administration

- Non-admin users cannot access `/api/v1/admin` operations.
- Administrators can manage products, stores, inventory, and order status.
- Administrative mutations create queryable audit records.

## Quality Gate

```powershell
dotnet build Selqi.slnx -c Release
dotnet test Selqi.slnx -c Release --no-build
Set-Location src/frontend/selqi-ui
npm ci
npm run build
npm test -- --watch=false
npm run e2e
```

The end-to-end command requires a running, migrated API/database. No real credentials or card data may appear in tracked files, generated artifacts, logs, container definitions, or documentation. The complete idempotent SQL script must include every EF migration and deterministic seed operation.
