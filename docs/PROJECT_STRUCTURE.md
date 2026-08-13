# Project Structure

```text
AI_Acceleration_Demo/
├── src/
│   ├── backend/
│   │   ├── Selqi.Domain/          Entities, enums, and domain constants
│   │   ├── Selqi.Application/     Use cases, DTOs, and interfaces
│   │   ├── Selqi.Infrastructure/  EF Core, repositories, and integrations
│   │   └── Selqi.Api/             Controllers, middleware pipeline, composition
│   └── frontend/selqi-ui/
│       ├── src/app/core/          Services, guards, and interceptors
│       ├── src/app/shared/        Reusable components and utilities
│       ├── src/app/features/      Route-level customer/admin features
│       └── e2e/                   Playwright scenarios
├── tests/backend/                 xUnit projects
├── database/                      SQL creation, migration, and seed script
├── scripts/                       Repeatable local setup helpers
└── docs/                          Product and engineering documentation
```

## Placement Rules

Keep business entities free of EF, ASP.NET Core, or UI concerns. Add application contracts before infrastructure implementations. Controllers should translate HTTP requests and responses, not implement business rules. Place feature-specific Angular code under `features/<feature>` and reusable singleton behavior under `core`.

Backend unit tests mirror the application capability, for example `Basket/BasketServiceTests.cs`. Frontend unit tests live beside the source as `*.spec.ts`; cross-page browser journeys belong in `e2e/`. Database model changes require an EF migration and regeneration of `database/migrations/001_complete_schema_and_seed.sql`.

Generated folders such as `bin`, `obj`, `node_modules`, `.angular`, `dist`, coverage output, logs, IDE state, and local secret files do not belong in the repository.
