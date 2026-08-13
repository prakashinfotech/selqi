# Database Design

## Platform and Ownership

Selqi uses SQL Server 2022 and EF Core 10. `SelqiDbContext` in `Selqi.Infrastructure` owns mappings and deterministic seed data. Migrations are the schema source of truth; `database/migrations/001_complete_schema_and_seed.sql` is the deployable idempotent representation.

## Schema Areas

| Area | Tables |
| --- | --- |
| Catalog | `Categories`, `Brands`, `Products`, `ProductImages`, `ProductSpecifications` |
| Stores | `Stores`, `Inventory` |
| Identity | `Users`, `Addresses`, `PasswordResetTokens` |
| Shopping | `Baskets`, `BasketItems`, `WishlistItems`, `SavedCards` |
| Orders | `Orders`, `OrderItems`, `OrderStatusHistories` |
| Operations | `AuditLogs`, `__EFMigrationsHistory` |

Product, address, and pricing details needed for historical orders are snapshotted into order records. Saved cards contain provider tokens and display metadata only; raw card numbers and CVVs must never be stored.

## Seed Data

EF migrations seed the demonstration catalog, category hierarchy, brands, product media/specifications, five stores, and per-store inventory. Stable GUIDs make repeatable environments deterministic. Users and administrator credentials are not seeded; register a user through the API and assign roles only through an authorized local administration process.

## Updating the Schema

```powershell
dotnet tool restore
dotnet tool run dotnet-ef migrations add <MigrationName> --project src/backend/Selqi.Infrastructure --startup-project src/backend/Selqi.Api
dotnet tool run dotnet-ef database update --project src/backend/Selqi.Infrastructure --startup-project src/backend/Selqi.Api
dotnet tool run dotnet-ef migrations script --idempotent --project src/backend/Selqi.Infrastructure --startup-project src/backend/Selqi.Api --output database/migrations/001_complete_schema_and_seed.sql
```

Commit the migration class, model snapshot, and regenerated SQL together. Validate upgrades against a disposable database before production use; do not edit an already-deployed migration solely to change a live schema.
