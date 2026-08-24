# Contributing to Selqi

Thank you for helping improve Selqi. This guide explains the expected development and pull-request workflow.

## Before You Start

- Read the project setup instructions in [README.md](README.md).
- Search existing issues and pull requests before starting duplicate work.
- For security vulnerabilities, follow [SECURITY.md](SECURITY.md) instead of opening a public issue.

## Development Setup

Fork the repository, clone your fork, and install the project dependencies:

```bash
git clone https://github.com/YOUR_USERNAME/selqi.git
cd selqi

dotnet restore Selqi.slnx

cd src/frontend/selqi-ui
npm ci
```

For the simplest complete environment, copy `.env.example` to `.env`, set a strong local `SELQI_DB_PASSWORD`, and run `docker compose up --build` from the repository root. Never commit real credentials.

## Contribution Workflow

1. Create a focused branch from the latest `master` branch:

   ```bash
   git checkout master
   git pull origin master
   git checkout -b feature/short-description
   ```

2. Follow the existing layered .NET architecture, Angular conventions, and `.editorconfig` rules.
3. Add or update tests whenever behavior changes.
4. Run the relevant quality checks.
5. Commit using a focused Conventional Commit such as `feat:`, `fix:`, `test:`, `docs:`, `chore:`, or `refactor:`.
6. Push your branch and open a pull request against `master`.

## Quality Checks

Run the backend checks from the repository root:

```bash
dotnet build Selqi.slnx --configuration Release
dotnet test Selqi.slnx --configuration Release --no-build
```

Run the frontend checks:

```bash
cd src/frontend/selqi-ui
npm ci
npm run build
npm test -- --watch=false
```

With the Docker stack running, verify the browser journey when the change affects customer or checkout behavior:

```bash
cd src/frontend/selqi-ui
E2E_BASE_URL=http://localhost \
E2E_API_BASE_URL=http://localhost/api/v1 \
npm run e2e
```

For dependency changes, also review known vulnerabilities:

```bash
dotnet list Selqi.slnx package --vulnerable --include-transitive

cd src/frontend/selqi-ui
npm audit
```

## Security and Sensitive Data

Do not commit:

- API keys, tokens, passwords, connection strings, or payment credentials
- `.env` files or environment-specific configuration
- private certificates, SSH keys, or signing material
- personal data, database exports, logs, or service-account credentials

Use `.env.example`, ASP.NET Core User Secrets, or environment variables with safe placeholders. If a secret is committed accidentally, rotate it immediately before requesting history cleanup.

## Pull Request Checklist

- [ ] The change is focused and documented.
- [ ] Relevant builds and tests pass locally.
- [ ] New or changed behavior has appropriate test coverage.
- [ ] No secret, credential, or private data is included.
- [ ] New configuration variables use safe placeholders and are documented.
- [ ] Database migrations and compatibility impacts are explained when applicable.
- [ ] UI changes include clear screenshots or an animated preview.
- [ ] The pull request explains what changed and how it was verified.

By contributing, you agree that your contribution may be used under the repository's [MIT License](LICENSE) and applicable company policies.
