# Selqi UI

Angular 21 standalone-component application for the Selqi ecommerce demonstration. Route features live in `src/app/features`, shared UI in `src/app/shared`, and singleton services, guards, and interceptors in `src/app/core`.

## Prerequisites

Use Node.js 22 and npm 11.12.1. The development UI expects the API at `http://localhost:5123/api/v1`; start SQL Server and `Selqi.Api` first for data-backed flows.

## Install and Run

```powershell
npm ci
npm start
```

Open `http://localhost:4200`. Production builds use the relative `/api/v1` URL so Nginx can proxy requests to the API container.

## Build and Unit Tests

```powershell
npm run build
npm test -- --watch=false
```

Angular CLI runs the `*.spec.ts` unit tests with Vitest. Keep tests beside their source and cover observable request/response behavior rather than implementation details.

## End-to-End Tests

The Playwright checkout scenario requires a running API with a migrated, seeded database. Install Chromium once, then run:

```powershell
npx playwright install chromium
npm run e2e
```

Playwright starts the Angular development server on port 4200. Set `E2E_BASE_URL` and `E2E_API_BASE_URL` to test an existing deployment; the defaults are `http://localhost:4200` and `http://localhost:5123/api/v1`.

## Formatting and Conventions

Use two-space indentation and lowercase kebab-case filenames. Prefer strict TypeScript, standalone components, signals for local state, typed service contracts, and the existing core services/interceptors. Prettier is available through the repository's configured toolchain.
