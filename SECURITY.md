# Security Policy

## Supported Version

Security fixes are applied to the latest code on the `master` branch. Older commits and external forks are not actively supported.

## Reporting a Vulnerability

Please do not report suspected vulnerabilities through a public GitHub issue, discussion, or pull request.

Use one of these private channels:

1. Submit a [private GitHub security advisory](https://github.com/prakashinfotech/selqi/security/advisories/new), if private vulnerability reporting is enabled.
2. Otherwise, email [info@prakashinfotech.com](mailto:info@prakashinfotech.com) with the subject **Selqi Security Report**.

Include as much of the following information as possible:

- affected component, endpoint, or file
- steps required to reproduce the issue
- expected and observed behavior
- potential impact
- proof-of-concept details with sensitive values removed
- any suggested mitigation

The maintainers will review the report, validate its impact, and coordinate remediation and disclosure when appropriate.

## Handling Secrets

Never include real API keys, passwords, tokens, certificates, payment credentials, deployment hooks, or database connection strings in an issue or pull request. If a credential may have been exposed, revoke or rotate it immediately; removing it from the latest commit is not sufficient.

## Deployment Responsibility

This repository is provided as an ecommerce showcase and local-development codebase. Teams deploying it are responsible for configuring production secrets, HTTPS, trusted origins, database access controls, payment and email integrations, monitoring, backups, and platform-specific security settings.
