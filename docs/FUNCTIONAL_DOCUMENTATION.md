# Selqi Ecommerce Portal — Functional Documentation

**Audience:** Clients, product owners, end users, business stakeholders  
**Version:** 1.0  
**Last Updated:** May 2026  

---

## Table of Contents

1. [What is Selqi Ecommerce Portal?](#1-what-is-selqi-ecommerce-portal)
2. [Who Uses This Platform?](#2-who-uses-this-platform)
3. [Core Features — Customer](#3-core-features--customer)
4. [Core Features — Administrator](#4-core-features--administrator)
5. [AI-Powered Recommendations](#5-ai-powered-recommendations)
6. [Account and Profile Management](#6-account-and-profile-management)
7. [Page-by-Page Feature Guide](#7-page-by-page-feature-guide)
8. [How Orders Work — End to End](#8-how-orders-work--end-to-end)
9. [Notifications and Emails](#9-notifications-and-emails)
10. [Store Finder](#10-store-finder)
11. [What Happens When Things Go Wrong](#11-what-happens-when-things-go-wrong)

---

## 1. What is Selqi Ecommerce Portal?

The Selqi Ecommerce Portal is a fully functional online retail platform that allows customers to browse, search, and purchase products across a wide range of categories — from electronics and home appliances to toys and outdoor furniture. The platform is designed to reflect the experience of the Selqi retail brand, combining an online shopping experience with visibility into physical store stock availability.

### Key Highlights

- **Browse** thousands of products across organised categories
- **Search and filter** by price, brand, availability, and offers
- **Buy online** with secure checkout and order tracking
- **Save favourites** to a wishlist for later
- **Reserve or check stock** at local stores
- **Personalised AI recommendations** powered by Google Gemini on the home page
- **Secure account management** — addresses, saved payment cards, order history
- **Admin portal** for full back-office control

---

## 2. Who Uses This Platform?

### Customer (Shopper)

Any person visiting the site can browse products and view prices. To add items to a basket, save a wishlist, or place an order, the customer must create a free account.

**What a customer can do:**
- Browse and search products freely (no account required)
- Register and log in securely
- Add products to basket and checkout
- Save and manage delivery addresses
- Save payment card details for faster checkout
- View full order history with status tracking
- Build a wishlist of favourite products
- Receive AI-personalised product suggestions

### Administrator

An administrator is an internal staff member with elevated access to the back-office management portal.

**What an administrator can do:**
- View a business dashboard with key metrics
- Create, update, and manage the product catalogue
- Manage physical store locations and inventory levels
- Update order statuses (e.g., Confirmed → Dispatched → Delivered)
- View a full audit log of all admin actions

---

## 3. Core Features — Customer

### 3.1 Product Discovery

| Feature | Description |
|---------|-------------|
| **Category browsing** | Products are organised in a hierarchical category structure. Click any category to filter the product grid instantly. |
| **Search** | Full-text search across product names, descriptions, and brands. |
| **Price filters** | Set a minimum and/or maximum price range. |
| **Brand filter** | Narrow results to one or more specific brands. |
| **Availability filter** | Show only products that are currently in stock. |
| **Offers filter** | Show only products with a sale/was-price discount applied. |
| **Sort options** | Sort by Top Rated, Price Low-to-High, Price High-to-Low, or Newest. |
| **Pagination** | Results load in pages (24 per page by default). Navigate with page controls. |

### 3.2 Product Detail

Each product has a dedicated page showing:
- Full product name, brand, and category
- All product images (scrollable gallery)
- Current price and "was" price with percentage saving (if on offer)
- Star rating and review count
- Product description and full technical specifications
- **Live stock availability by store** — see which physical stores have the item in stock

### 3.3 Shopping Basket

- Add any in-stock product to the basket from the product detail page
- View basket with itemised list, unit prices, and line totals
- Change item quantities or remove items
- See the running total with delivery charge calculated automatically
- **Free standard delivery** on orders over £50
- Express delivery option available at £5.95

### 3.4 Checkout

A single-step checkout flow:
1. Select or enter a delivery address
2. Select a saved payment card or enter new card details
3. Choose delivery method (Standard / Express)
4. Review order summary
5. Confirm and place order

On successful order placement, the user is taken to an order confirmation page with the unique order number.

### 3.5 Order History and Tracking

- View all past and current orders in a list
- Each order shows: order number, date placed, status, item count, and total
- Open any order to see full detail: itemised products, delivery address, payment reference, and status timeline

**Order Status Progression:**

```
Placed → Confirmed → Processing → Dispatched → Delivered
```

Cancelled orders are shown with a "Cancelled" badge.

### 3.6 Wishlist

- Add any product to your wishlist from the product detail page
- View and manage the wishlist from the "Wishlist" section in your account
- Remove items from the wishlist at any time
- Move items from wishlist to basket directly

---

## 4. Core Features — Administrator

The admin portal is accessible only to users with the Administrator role. It is reached via `/admin`.

### 4.1 Dashboard

A summary view showing:
- Total number of products, stores, orders
- Recent activity snapshot

### 4.2 Product Management

- View a complete list of all products in the catalogue
- Create new products with: name, SKU, category, brand, price, was-price, images, specifications, and description
- Edit any existing product details
- Soft-delete products (they are hidden from customers but preserved in order history)

### 4.3 Store Management

- View all physical store locations with their address, type, and status
- Create new store locations
- Update store details
- Adjust inventory (stock levels) per product per store
- Deactivate or remove stores

### 4.4 Order Management

- View all orders placed across all customers
- Filter and search orders
- Update order status at each stage of fulfilment
- Attach a note when changing status (appears in the customer's order timeline)

### 4.5 Audit Log

Every admin action is automatically recorded including:
- Who performed the action (email and user ID)
- What was done (e.g., "Updated product", "Changed order status")
- Which entity was affected
- When it happened

This log is read-only and cannot be edited or deleted.

---

## 5. AI-Powered Recommendations

The home page features a section called **"AI Picked for You"** powered by Google Gemini.

### How It Works for the Customer

- When you visit the home page, the system analyses your purchase history (if you are logged in) or selects from a curated pool of popular products (if you are a new visitor)
- Six products are recommended, each with a short, personalised reason explaining why that product was selected for you
- Recommendations refresh every 30 minutes

### What the Customer Sees

Each recommendation card shows:
- Product image, name, and price
- An "AI Picked for you" badge
- "Powered by Gemini" label
- A short reason sentence (e.g., "Based on your interest in home appliances, this energy-efficient air fryer would be a great fit.")

### Personalisation

- **Logged-in customers:** Recommendations are based on your last 5 orders' product history
- **New visitors / not logged in:** Recommendations are drawn from a broad pool of popular, in-stock products

### Availability

If the AI service is temporarily unavailable, the section will display a friendly message: _"AI suggestions unavailable right now."_ The rest of the page continues to work normally.

---

## 6. Account and Profile Management

### Registration

To create an account, provide:
- First and last name
- Email address (must be unique)
- Password (minimum length enforced)

On successful registration, you are automatically logged in and taken to the home page.

### Login

Login requires your email address and password. A JWT token is issued and stored in your browser for 24 hours. You remain logged in until you log out or the session expires.

### Password Reset

If you forget your password:
1. Click "Forgot Password" on the login page
2. Enter your registered email address
3. Check your email for a secure reset link (single-use, time-limited)
4. Click the link and set a new password

### Delivery Addresses

- Save multiple delivery addresses to your account
- Label them for easy identification (e.g., "Home", "Work")
- Mark one as the default — it will be pre-selected at checkout
- Edit or delete addresses at any time

### Saved Payment Cards

- Save card details for faster checkout (tokenised — full card numbers are never stored)
- Mark a default card
- Remove saved cards at any time

---

## 7. Page-by-Page Feature Guide

### Home Page (`/`)

| Section | What It Does |
|---------|-------------|
| **Quick Strip** | Category shortcuts and special tiles (0% Finance, Top Rated, Ready to Collect) |
| **Hero Slider** | Auto-rotating banner with promotional messages and calls to action |
| **Trending Now** | Four featured category tiles linking to filtered product grids |
| **Popular Products** | Top-rated products across all categories |
| **AI Picked for You** | Personalised Gemini recommendations with reasons |
| **Spring Deals** | Seasonal category highlights |
| **Explore Brands** | Quick links to browse by brand |
| **More Services** | Checkout, inventory, orders, and wishlist info tiles |
| **Flexible Payment** | Selqi Pay finance banner |
| **Store Finder** | Postcode search form to locate nearby stores |

### Catalog Page (`/products`)

- Full-width product grid
- Left sidebar with category, brand, price, and availability filters
- Sort control in the top-right
- Pagination controls at the bottom
- URL reflects all active filters — pages are shareable and bookmarkable

### Product Detail Page (`/product/:slug`)

- Product gallery (multiple images)
- Price, was-price, and discount percentage
- Add to Basket and Add to Wishlist buttons
- Technical specifications table
- In-store availability showing stock quantities per location

### Basket Page (`/basket`)

- Requires login
- Itemised list of products with quantity selectors
- Remove individual items or clear the entire basket
- Delivery cost preview
- "Proceed to Checkout" button

### Checkout Page (`/checkout`)

- Requires login
- Address selector (or manual entry)
- Delivery method selection
- Payment card selector
- Order summary
- Place Order button

### Order Confirmation and History (`/orders`)

- List of all orders with status badge
- Click any order to see full detail and status timeline

### Wishlist Page (`/wishlist`)

- Requires login
- Grid of saved products
- Quick-add to basket from wishlist
- Remove individual items

### Admin Pages (`/admin`)

- Requires admin account
- Dashboard, Products, Stores, Orders, Audit Log — all accessible via the sidebar navigation

---

## 8. How Orders Work — End to End

```
Customer adds items to basket
        ↓
Customer proceeds to checkout
  → Selects delivery address
  → Selects payment method
  → Chooses delivery speed
        ↓
Customer clicks "Place Order"
        ↓
System creates the order with status: Placed
  → Saves itemised snapshot (prices, names, images)
  → Calculates delivery charge
  → Clears the basket
        ↓
Customer sees Order Confirmation page with order number
        ↓
Admin updates order status:
  Placed → Confirmed → Processing → Dispatched → Delivered
  (each change is timestamped and logged)
        ↓
Customer can view live order status at any time in Order History
```

---

## 9. Notifications and Emails

| Event | Email Sent | To |
|-------|-----------|-----|
| Password reset requested | Secure reset link email | Customer |

Additional notification events (order confirmation email, dispatch notification) are planned for future implementation.

---

## 10. Store Finder

The home page includes a Store Finder panel where customers can enter a postcode to find nearby Selqi store locations.

Each store record in the system contains:
- Store name and full address
- Contact phone number
- Store type (Retail Store or Collection Point)
- Geographic coordinates (latitude/longitude)
- Active/inactive status

In-store product availability is displayed on each product's detail page, showing which stores have stock and the quantity available.

---

## 11. What Happens When Things Go Wrong

The platform is designed to handle errors gracefully so customers are never left with a blank screen or a confusing error code.

| Scenario | What the Customer Sees |
|----------|----------------------|
| Product not found | A "Product not found" message with a link back to the catalogue |
| Empty search results | An "No products match your filters" message with a suggestion to broaden the search |
| AI recommendations unavailable | The AI section shows: "AI suggestions unavailable right now" — the rest of the page works normally |
| Session expired | Automatically redirected to the login page; after logging in, returned to where they were |
| Checkout fails | An error message with guidance on what to try next; basket items are preserved |
| Page loading | Skeleton placeholder cards are shown while content loads — no blank flashes |
| Admin action fails | Inline error messages with details; no data is lost |
