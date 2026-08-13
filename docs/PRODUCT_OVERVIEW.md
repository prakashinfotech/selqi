# Selqi Ecommerce Portal — Product Overview

**Audience:** Business leadership, investors, non-technical stakeholders, new team members  
**Version:** 1.0  
**Last Updated:** May 2026  

---

## What is the Selqi Ecommerce Portal?

The Selqi Ecommerce Portal is a modern, full-featured online retail platform built to replicate and extend the Selqi retail experience in the digital space. It provides customers with a seamless end-to-end shopping journey — from discovering products through AI-powered recommendations to checking out and tracking their orders — all within a fast, secure, and easy-to-use web application.

Built on contemporary technology, the platform is designed for real-world scalability and has been developed with the same engineering standards and architectural patterns found in leading ecommerce companies.

---

## The Problem It Solves

Traditional retail is increasingly moving online. Customers expect to:

- **Find products instantly** — not browse aisles or call a store
- **Check stock availability** — before making a trip to collect
- **Shop on any device** — phone, tablet, or desktop
- **Receive personalised suggestions** — relevant to their taste and purchase history
- **Manage everything in one place** — their orders, addresses, and payment methods

The Selqi Ecommerce Portal answers all of these expectations with a cohesive, integrated platform.

---

## Key Value Propositions

### For Customers

| Value | How It's Delivered |
|-------|--------------------|
| **Easy product discovery** | Full search, multi-filter catalogue, category browsing |
| **Personalised experience** | AI-powered "Picked for You" recommendations using Google Gemini |
| **Transparent stock visibility** | Real-time store-level stock shown on every product page |
| **Secure and fast checkout** | Saved addresses, saved cards, one-click reorder |
| **Peace of mind** | Order history, status tracking, and status timeline for every order |
| **Trusted security** | Passwords never stored in plain text; payment cards tokenised |

### For the Business

| Value | How It's Delivered |
|-------|--------------------|
| **Full catalogue control** | Admin portal to create, update, and manage all products |
| **Inventory management** | Per-store stock levels updated by administrators in real time |
| **Order fulfilment visibility** | Full order pipeline view with status management and notes |
| **Audit compliance** | Every admin action permanently logged with who, what, and when |
| **Scalable architecture** | Clean layered architecture designed to grow with the business |

---

## How the Platform Works — Simple Overview

```
Customer visits the website
        ↓
Browses products by category, search, or AI picks
        ↓
Views product detail including prices, images, specs, and store stock
        ↓
Creates an account (or logs in)
        ↓
Adds products to their basket
        ↓
Checks out: selects address, payment card, delivery speed
        ↓
Order is placed, confirmed, and tracked
        ↓
Administrator manages fulfilment in the back-office
        ↓
Customer receives order — status updated throughout
```

---

## The AI Advantage

One of the platform's standout features is its **AI-powered recommendation engine**, driven by Google's Gemini language model.

When a customer visits the home page, the system:
1. Looks at the customer's recent purchase history (if they are logged in)
2. Selects from a pool of the newest in-stock products
3. Asks Gemini AI to recommend six products that best fit that customer's profile
4. Displays each product with a personalised, plain-English reason for the suggestion

This is not generic "customers also bought" logic — it is contextual, conversational AI reasoning applied to product curation, refreshing every 30 minutes to stay current.

For new or anonymous visitors, the AI still runs — drawing on the product pool to surface varied, interesting products rather than just the best-sellers.

---

## Who Is It For?

### Shoppers (Customers)

Anyone looking to browse and purchase Selqi-range products online. No technical knowledge required — the interface is designed for everyday use.

**Typical customer journey time:** 5–10 minutes from product search to order placement.

### Business Administrators

Internal staff who manage the product catalogue, store inventory, and order pipeline. Access is via a separate admin portal with role-based access control.

### Technical Teams

Developers, architects, and DevOps engineers responsible for maintaining, extending, and operating the platform. The clean architecture and documented codebase are designed to make onboarding and feature development fast.

---

## What the Platform Includes Today

| Feature Area | Status |
|-------------|--------|
| Product catalogue (search, filter, sort, detail) | Live |
| Category and brand browsing | Live |
| Store inventory visibility | Live |
| Customer registration and login | Live |
| Password reset via email | Live |
| Shopping basket | Live |
| Secure checkout | Live |
| Order history and tracking | Live |
| Wishlist | Live |
| Saved delivery addresses | Live |
| Saved payment cards (tokenised) | Live |
| Razorpay sandbox checkout | Live when configured |
| AI-powered recommendations (Gemini) | Live |
| Admin portal (products, stores, orders, audit) | Live |
| PDF invoice generation | Infrastructure complete |
| Order confirmation emails | Planned |

---

## Technology at a Glance

The platform is built with modern, mainstream technologies that are well-supported and widely understood:

| Layer | Technology | Why |
|-------|-----------|-----|
| Customer-facing web app | Angular 21 | Fast, reactive, modern component model |
| Business logic and API | ASP.NET Core (.NET 10) | Enterprise-grade, high-performance, cross-platform |
| Database | SQL Server | Reliable, ACID-compliant, industry-standard |
| AI recommendations | Google Gemini | State-of-the-art language model, REST API, generous free tier |
| Email | Office 365 SMTP | Leverages existing business infrastructure |
| PDF generation | QuestPDF | Pure .NET, no external services |

---

## Security and Trust

Security was treated as a first-class requirement throughout the build:

- **Passwords** are never stored — only a one-way hash is kept
- **Payment cards** are handled only by the local demonstration flow or Razorpay sandbox; the API stores display metadata and a local reference token, never PAN or CVV
- **Every API request** to protected resources requires a cryptographically signed token
- **Admin actions** are permanently logged for accountability
- **Password reset links** expire and can only be used once
- **User data isolation** — customers can only access their own orders, addresses, and cards

---

## Deployment Options

The platform is designed to run in multiple environments:

- **Local development** — runs on a developer's machine with a local SQL Server in minutes
- **Docker containers** — backend auto-applies database migrations on startup
- **Cloud hosting** — fully compatible with Azure, AWS, or GCP (App Service, ECS, Cloud Run)
- **Managed database** — works with Azure SQL, Amazon RDS, or any SQL Server-compatible service

---

## How It Grows

The architecture has been deliberately designed so that new features slot in cleanly without disrupting existing functionality:

- Adding a new feature = add a domain entity + application service + API controller + frontend component
- Swapping the AI provider = implement a new class for `IAiRecommendationService`
- Scaling horizontally = stateless API allows multiple instances behind a load balancer (cache would move to Redis)
- Adding another payment provider = implement its server-side order, verification, and reconciliation flow behind the existing payment boundary

The platform is a maintainable demonstration foundation. Production use still requires payment, security, privacy, resilience, monitoring, and operational hardening.
