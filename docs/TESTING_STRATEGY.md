# Testing Strategy

## Test Suites

- `tests/backend/Selqi.Application.Tests` uses xUnit for authentication, password hashing, basket, and order application services.
- Angular `*.spec.ts` files use Vitest through Angular CLI for the root component and catalog, basket, order, and Razorpay services.
- `src/frontend/selqi-ui/e2e/checkout.spec.ts` uses Playwright/Chromium for the browse-to-checkout journey.

## Run Backend Tests

```powershell
dotnet restore Selqi.slnx
dotnet build Selqi.slnx -c Release --no-restore
dotnet test Selqi.slnx -c Release --no-build
```

## Run Frontend Tests

```powershell
Set-Location src/frontend/selqi-ui
npm ci
npm run build
npm test -- --watch=false
```

## Run End-to-End Tests

Start SQL Server and the API first. Playwright starts the Angular development server automatically.

```powershell
Set-Location src/frontend/selqi-ui
npx playwright install chromium
npm run e2e
```

Use `E2E_BASE_URL` and `E2E_API_BASE_URL` to target an existing UI and API. For the Docker stack, set them to `http://localhost` and `http://localhost/api/v1`. The test requires a reachable API with migrated seed data.

## Expectations

There is no enforced numeric coverage threshold. Every behavior change should add a focused regression test covering the success path and relevant validation, authorization, ownership, and failure paths. Name C# suites `*Tests.cs`, frontend unit tests `*.spec.ts`, and keep browser tests under `e2e/`. Builds, unit tests, and applicable Playwright scenarios must pass before publication.
