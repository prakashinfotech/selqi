# Selqi Database

Selqi uses SQL Server 2022 through Entity Framework Core 10. The database assets are safe for repeatable local setup and contain no credentials.

## Included Scripts

- `000_create_database.sql` creates the target database when it does not exist.
- `migrations/001_complete_schema_and_seed.sql` is an idempotent script generated from all twelve EF migrations.
- `../scripts/init-database.ps1` runs both scripts and stops on the first SQL error.

The migration script creates the complete catalog, identity, shopping, order, audit, password-reset, saved-card, store, and inventory schema. It seeds categories, brands, stores, products, product images, specifications, and inventory. It deliberately does not seed login credentials.

## Initialize with Integrated Authentication

From the repository root:

```powershell
.\scripts\init-database.ps1 -ServerInstance "localhost\SQLEXPRESS" -DatabaseName SelqiDb
```

## Initialize with SQL Authentication

Keep the password outside shell history where possible. The helper reads `SELQI_DB_PASSWORD` by default:

```powershell
$env:SELQI_DB_PASSWORD = "<local-password>"
.\scripts\init-database.ps1 -ServerInstance localhost -DatabaseName SelqiDb -User sa
```

## Apply or Create EF Migrations

```powershell
dotnet tool restore
dotnet tool run dotnet-ef database update --project src/backend/Selqi.Infrastructure --startup-project src/backend/Selqi.Api
```

Create a new migration after changing the EF model:

```powershell
dotnet tool run dotnet-ef migrations add <MigrationName> --project src/backend/Selqi.Infrastructure --startup-project src/backend/Selqi.Api
```

Regenerate the checked-in idempotent script after every migration:

```powershell
dotnet tool run dotnet-ef migrations script --idempotent --project src/backend/Selqi.Infrastructure --startup-project src/backend/Selqi.Api --output database/migrations/001_complete_schema_and_seed.sql
```

Review generated SQL before applying it outside local development and back up existing data before schema changes.
