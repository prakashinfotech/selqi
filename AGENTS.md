# Repository Guidelines

## Project Structure & Module Organization

`src/backend/` contains the .NET 10 layers: `Selqi.Domain` for entities, `Selqi.Application` for use cases and interfaces, `Selqi.Infrastructure` for persistence and integrations, and `Selqi.Api` for controllers and composition. Keep domain code framework-independent.

The Angular 21 client lives in `src/frontend/selqi-ui/`: feature UI is under `src/app/features`, reusable code under `shared`, and services, guards, and interceptors under `core`. Backend tests are in `tests/backend/Selqi.Application.Tests`; frontend unit tests sit beside source, and Playwright scenarios are in `e2e/`. Put SQL in `database/`, helpers in `scripts/`, and technical decisions in `docs/`.

## Build, Test, and Development Commands

- `dotnet restore Selqi.slnx` — restore backend and test dependencies.
- `dotnet build Selqi.slnx -c Release` — compile the full backend solution.
- `dotnet run --project src/backend/Selqi.Api` — start the API locally.
- From `src/frontend/selqi-ui`, run `npm ci`, then `npm start` — install dependencies and serve Angular.
- `dotnet test Selqi.slnx -c Release` — run all xUnit tests.
- `npm test -- --watch=false` — run frontend Vitest tests once; `npm run e2e` runs Playwright.
- `docker compose up --build` — start SQL Server, API, and web containers together.

## Coding Style & Naming Conventions

Follow `.editorconfig`: UTF-8, CRLF, final newline, four-space C# indentation, and two-space frontend indentation. Use PascalCase for C# types and public members, `I` prefixes for interfaces, and `Async` suffixes for asynchronous methods. Use lowercase kebab-case Angular filenames. Prefer strict typing, dependency injection, and existing constants/services. Prettier is available for frontend formatting.

## Testing Guidelines

Use xUnit, Vitest, and Playwright. Name C# suites `*Tests.cs` and TypeScript tests `*.spec.ts`. Cover success, validation, authorization, and failure paths; prioritize catalog, basket, authentication, checkout, payment, and order flows. Add a regression test for every bug fix.

## Commit & Pull Request Guidelines

Create short-lived branches such as `feature/admin-operations`, `hotfix/basket-nullability`, or `test/critical-flow-coverage`. Use focused Conventional Commits: `feat:`, `fix:`, `test:`, `docs:`, `chore:`, or `refactor:`. Pull requests should explain the change, link the issue, list verification, and include screenshots for UI changes. Ensure relevant builds and tests pass before review.

## Security & Configuration

Never commit secrets, real payment credentials, database backups, generated logs, or environment-specific settings. Supply connection strings, JWT keys, SMTP credentials, Gemini keys, and Razorpay credentials through local configuration or environment variables.
