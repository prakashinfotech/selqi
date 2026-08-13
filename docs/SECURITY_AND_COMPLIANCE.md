# Security and Configuration

## Secret Handling

Tracked configuration contains placeholders only. Never commit connection passwords, JWT signing secrets, SMTP passwords, Razorpay secrets, Gemini keys, customer data, database backups, or local `.env` files.

Use ASP.NET Core User Secrets for manual development:

```powershell
dotnet user-secrets set "Jwt:Secret" "<random-secret>" --project src/backend/Selqi.Api
dotnet user-secrets set "Email:Password" "<smtp-password>" --project src/backend/Selqi.Api
dotnet user-secrets set "Razorpay:KeyId" "<sandbox-key-id>" --project src/backend/Selqi.Api
dotnet user-secrets set "Razorpay:KeySecret" "<sandbox-secret>" --project src/backend/Selqi.Api
dotnet user-secrets set "Gemini:ApiKey" "<api-key>" --project src/backend/Selqi.Api
```

Local administrator creation is disabled by default. If it is explicitly enabled, supply `AdminSeed:Email` and `AdminSeed:Password` through User Secrets and disable it again after initialization; never place the password in JSON.

Docker Compose reads only `SELQI_DB_PASSWORD` from the ignored root `.env`. In hosted environments, use the platform secret store and standard double-underscore environment keys such as `ConnectionStrings__SelqiDb` and `Jwt__Secret`.

## Application Controls

- Passwords are one-way hashed; the API issues signed, expiring JWTs.
- Customer resources require authentication and are scoped to the current user.
- Administrator operations require the `Admin` role and produce audit records.
- CORS uses an explicit origin list. Production traffic must use HTTPS.
- Password-reset tokens are time-limited and stored separately from passwords.

## Payment and Personal Data

Use Razorpay sandbox credentials during development. Verify payment signatures on the server and never accept client-side success as proof of payment. Saved-card records may contain provider tokens, brand, expiry display data, and last four digits; do not log or persist PAN, CVV, or raw provider secrets.

Before public deployment, rotate any previously shared credentials, scan the repository and container images for secrets, restrict database/network access, define retention rules for users, addresses, orders, and audit data, and confirm applicable privacy and payment-compliance obligations.
