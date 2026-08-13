# Deployment Guide

## Local Docker Deployment

Docker Compose is the supported reproducible environment. It builds the Angular/Nginx site and .NET API, starts SQL Server 2022, waits for database health, and lets the API apply all EF migrations.

```powershell
Copy-Item .env.example .env
# Set a strong SELQI_DB_PASSWORD in .env
docker compose up --build -d
docker compose ps
Invoke-WebRequest http://localhost:5000/health
```

Use `http://localhost` for the application. View container output with `docker compose logs -f`; stop without deleting data using `docker compose down`, or explicitly add `--volumes` only when a disposable database should be removed.

## Manual Development

Configure a SQL Server connection and JWT secret with User Secrets, apply migrations, then run the API and frontend as described in the root README. The Angular development server uses `http://localhost:5123/api/v1`; the production build uses `/api/v1` behind Nginx.

## Production Configuration

Supply configuration from the target platform's secret store or environment variables. Required production values include:

- `ConnectionStrings__SelqiDb`
- `Jwt__Secret`, `Jwt__Issuer`, and `Jwt__Audience`
- `Cors__AllowedOrigins__0` and additional origins as needed
- SMTP, Razorpay, and Gemini settings when those integrations are enabled

Do not deploy the dummy values in tracked `appsettings*.json` files. Use TLS at the ingress, a least-privileged database account, persistent SQL backups, and health probes against `/health`. Run the Release build, all unit tests, an idempotent migration test against a disposable database, and critical Playwright scenarios before promotion.

## Database Rollout

The API auto-migrates only in the `Docker` environment. For controlled environments, review and execute `database/migrations/001_complete_schema_and_seed.sql` as a deployment step before starting the API. Back up existing data and test both fresh installation and upgrade paths.
