# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Selqi Ecommerce Portal** — an 8-day practical build of a full-stack ecommerce platform.

- **Frontend**: Angular 21 standalone components in `src/frontend/selqi-ui/`
- **Backend**: ASP.NET Core .NET 10 Web API (clean architecture) in `src/backend/`
- **Database**: SQL Server with Entity Framework Core migrations
- **API**: RESTful endpoints under `/api/v1/`

Days 1–2 are complete (catalog DB, seed data, read APIs). Days 3–8 (auth, basket, checkout, orders, admin, wishlist) are not yet implemented.

## Build & Run Commands

### Backend

```powershell
dotnet restore
dotnet build
dotnet run --project src/backend/Selqi.Api/Selqi.Api.csproj
# API: http://localhost:5123  |  HTTPS: https://localhost:7189
# Health: http://localhost:5123/health
```

### Database Migrations

```powershell
dotnet tool restore
dotnet ef database update --project src/backend/Selqi.Infrastructure --startup-project src/backend/Selqi.Api
```

### Frontend

```powershell
cd src/frontend/selqi-ui
npm install
npm start        # http://localhost:4200
npm run build    # production build
npm test         # Vitest unit tests
```

## Architecture

### Backend (4-layer clean architecture)

| Layer | Project | Responsibility |
|---|---|---|
| Domain | `Selqi.Domain` | Pure entities (`Product`, `Category`, `Brand`, `Store`, `Inventory`) — no framework deps |
| Application | `Selqi.Application` | Services, DTOs, repository interfaces, DI config |
| Infrastructure | `Selqi.Infrastructure` | `SelqiDbContext`, EF repository implementations, seed data |
| API | `Selqi.Api` | Thin controllers, `Program.cs`, middleware, OpenAPI |

Each layer has its own `DependencyInjection.cs` for service registration. Controllers delegate immediately to application services; no business logic in controllers.

### Repository Pattern

- Interfaces live in `Selqi.Application` (e.g., `ICatalogReadRepository`)
- EF implementations live in `Selqi.Infrastructure` (e.g., `EfCatalogReadRepository`)
- LINQ queries belong in repositories, not in services

### Frontend

Feature-first folder layout under `src/app/`:
- `features/`: `home/`, `catalog/`, `product/`, `basket/`, `checkout/`, `account/`, `orders/`, `admin/`
- `core/`: singleton services, guards, HTTP interceptors
- `shared/`: reusable components, pipes, directives

## Key Backend Patterns

- All service/repository methods are `async Task` with `CancellationToken`
- API responses always use DTOs (never expose EF entities)
- `PagedResultDto<T>` shape: `{ items, page, pageSize, totalItems, totalPages }` — default page size 24, max 100
- Input strings are normalized (lowercase + trim) before repository calls
- Catalog entities have `IsDeleted`, `CreatedAtUtc`, `UpdatedAtUtc` fields
- Money fields use `decimal(18,2)`

## Key Frontend Patterns

- Strict TypeScript: `noImplicitOverride`, `strictTemplates`, `noImplicitReturns` are all on
- Every data-bound page must handle loading, empty, error, and success states
- Query parameters (not route params) for filters, sorting, and pagination — keep URLs shareable
- Component styles in separate `.scss` files; global styles in `styles.scss`

## Adding a New API Endpoint

1. Create DTO(s) in `Selqi.Application/{Feature}/`
2. Add method to the feature service interface and implement it
3. Add repository interface method in `Selqi.Application` and implement in `Selqi.Infrastructure`
4. Add controller action in `Selqi.Api/Controllers/` with `[ProducesResponseType]` attributes

## Adding a Database Table

1. Create domain entity in `Selqi.Domain/{Feature}/`
2. Add `DbSet<Entity>` and configure in `SelqiDbContext.OnModelCreating`
3. `dotnet ef migrations add {Name} --project src/backend/Selqi.Infrastructure --startup-project src/backend/Selqi.Api`
4. Review the generated migration, then `dotnet ef database update ...`

## Environment

- SQL Server: local instance with Windows auth (connection string key: `SelqiDb`)
- CORS: `http://localhost:4200` and `https://localhost:4200` are allowed
- .NET 10 SDK required
- Node.js LTS required (npm 11.12.1 pinned in `package.json`)

## Docs

All planning docs are in `docs/`:
- `ARCHITECTURE.md` — layering rules and patterns
- `API_DESIGN.md` — endpoint contracts and response shapes
- `DATABASE_DESIGN.md` — schema, relationships, seed data
- `UI_IMPLEMENTATION_GUIDE.md` — frontend page requirements and folder structure
- `TESTING_STRATEGY.md` — test types and coverage priorities
- `SECURITY_AND_COMPLIANCE.md` — auth, secrets, payment safety

## Git Conventions

- Conventional commits: `feat:`, `fix:`, `test:`, `docs:`, `chore:`, `refactor:`
- Feature branches: `feature/day-03-auth`, `feature/catalog-api`
- Each commit = one logical change
