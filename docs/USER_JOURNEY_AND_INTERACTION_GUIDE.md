# Selqi Ecommerce Portal — User Journey and Interaction Guide

**Audience:** UX designers, product managers, QA testers, client stakeholders  
**Purpose:** Screen-by-screen walkthrough of every user interaction path with expected outcomes  
**Version:** 1.0  
**Last Updated:** May 2026  

---

## Table of Contents

1. [Journey 1: First-Time Visitor Browsing](#journey-1-first-time-visitor-browsing)
2. [Journey 2: Customer Registration and Login](#journey-2-customer-registration-and-login)
3. [Journey 3: Searching and Filtering Products](#journey-3-searching-and-filtering-products)
4. [Journey 4: Viewing a Product and Checking Stock](#journey-4-viewing-a-product-and-checking-stock)
5. [Journey 5: Adding to Basket and Checking Out](#journey-5-adding-to-basket-and-checking-out)
6. [Journey 6: Tracking an Order](#journey-6-tracking-an-order)
7. [Journey 7: Managing a Wishlist](#journey-7-managing-a-wishlist)
8. [Journey 8: Account and Password Management](#journey-8-account-and-password-management)
9. [Journey 9: Admin — Managing Products](#journey-9-admin--managing-products)
10. [Journey 10: Admin — Managing Orders](#journey-10-admin--managing-orders)
11. [Journey 11: AI Recommendations Experience](#journey-11-ai-recommendations-experience)
12. [Error States and Edge Cases](#error-states-and-edge-cases)
13. [Interaction States Reference](#interaction-states-reference)

---

## Journey 1: First-Time Visitor Browsing

**User:** Anonymous (not logged in)  
**Goal:** Explore the site without committing to registration

### Steps

| Step | User Action | What Happens | Screen Element |
|------|-------------|-------------|----------------|
| 1 | Opens `http://localhost:4200` | Home page loads | Hero slider appears; category strip loads |
| 2 | Sees the hero slider auto-rotating | Slides cycle every 5 seconds automatically | Slider with 3 promotional slides |
| 3 | Clicks a category tile in the Quick Strip | Navigates to `/products?category=<slug>` | Filtered product grid for that category |
| 4 | Scrolls through "Popular Products" section | 6 top-rated products displayed with prices and images | Product cards with star ratings |
| 5 | Scrolls to "AI Picked for You" section | 6 AI-recommended products load (slight delay) | Cards with "AI Picked for you" badge and reason text |
| 6 | Clicks any product card | Navigates to `/product/<slug>` | Product detail page |
| 7 | Tries to "Add to Basket" | Redirected to `/login` | Login page with return URL |
| 8 | Returns to home by clicking the Selqi logo | Back on home page | Header logo link |

**Expected Outcome:** Visitor can browse freely. Purchase actions prompt login but do not block browsing.

---

## Journey 2: Customer Registration and Login

### 2a: New Customer Registration

**User:** New visitor  
**Goal:** Create an account

| Step | User Action | What Happens | Validation |
|------|-------------|-------------|------------|
| 1 | Clicks "Sign In / Register" in header | Navigates to `/login` | — |
| 2 | Clicks "Create an account" link | Navigates to `/register` | — |
| 3 | Fills in First Name, Last Name, Email, Password | Form fields update | Client-side validation as user types |
| 4 | Submits with empty required field | Error message shown inline | "This field is required" |
| 5 | Submits with invalid email format | Error message shown | "Please enter a valid email" |
| 6 | Submits with valid data | API call made; JWT returned | — |
| 7 | Registration succeeds | Redirected to home page, logged in | Header shows user's name |
| 8 | Tries to register with an existing email | Error message displayed | "An account with this email already exists" |

### 2b: Returning Customer Login

| Step | User Action | What Happens |
|------|-------------|-------------|
| 1 | Navigates to `/login` | Login form displayed |
| 2 | Enters email and password | — |
| 3 | Submits with wrong password | Error: "Invalid email or password" |
| 4 | Submits with correct credentials | JWT stored in `localStorage`; redirected |
| 5 | JWT persists across browser refresh | Stays logged in (24-hour session) |
| 6 | Clicks "Sign Out" | JWT cleared; redirect to home; header reverts to "Sign In" |

---

## Journey 3: Searching and Filtering Products

**User:** Any (logged in or anonymous)  
**Goal:** Find specific products

### URL Pattern
All filters are query parameters: `/products?search=airpods&category=electronics&minPrice=50&maxPrice=200&sort=rating`

| Step | User Action | URL Change | Products Shown |
|------|-------------|-----------|----------------|
| 1 | Types "air fryer" in search box, presses Enter | `?search=air+fryer` | Matching products |
| 2 | Selects "Kitchen Appliances" category from filter | `?search=air+fryer&category=kitchen-appliances` | Further narrowed |
| 3 | Sets price range £30–£150 | `?minPrice=30&maxPrice=150` added | Price-filtered results |
| 4 | Ticks "In Stock Only" | `?availableOnly=true` added | Only stocked items |
| 5 | Ticks "Offers Only" | `?offersOnly=true` added | Only sale items |
| 6 | Changes sort to "Price: Low to High" | `?sort=price-asc` | Reordered by price |
| 7 | Clicks page 2 | `?page=2` | Next 24 products |
| 8 | Copies URL and opens in new tab | Same filters applied instantly | Identical results |

**Empty State:** If no products match, the page shows: _"No products match your current filters. Try broadening your search."_

---

## Journey 4: Viewing a Product and Checking Stock

**User:** Any  
**Goal:** Evaluate a specific product before purchasing

| Step | User Action | What They See |
|------|-------------|--------------|
| 1 | Clicks a product card anywhere on site | Navigates to `/product/<slug>` |
| 2 | Page loads | Hero image, name, brand, category shown |
| 3 | Scrolls through image gallery | Multiple product images shown; click to enlarge |
| 4 | Reads the price section | Current price; "Was £X" with % saving if on offer |
| 5 | Reads star rating and review count | e.g., "★★★★☆ 4.2 (847 reviews)" |
| 6 | Reads description | Short product description text |
| 7 | Expands specifications table | Technical specs listed (Name → Value pairs) |
| 8 | Scrolls to "Check In-Store Availability" | Table of stores with town, postcode, and quantity in stock |
| 9 | Clicks "Add to Basket" (logged in) | Product added; basket count in header increments |
| 10 | Clicks "Add to Basket" (not logged in) | Redirected to login page |
| 11 | Clicks "Add to Wishlist" (logged in) | Product added to wishlist; button state changes |

---

## Journey 5: Adding to Basket and Checking Out

**User:** Logged in customer  
**Goal:** Purchase products

### 5a: Building the Basket

| Step | User Action | What Happens |
|------|-------------|-------------|
| 1 | Clicks "Add to Basket" on a product | `POST /api/v1/basket/items` called |
| 2 | Clicks basket icon in header | Navigates to `/basket` |
| 3 | Sees all added items with quantities | Item list with unit price and line total |
| 4 | Clicks "+" or "-" on quantity | `PUT /api/v1/basket/items/{id}` updates quantity |
| 5 | Clicks bin icon on an item | `DELETE /api/v1/basket/items/{id}` removes item |
| 6 | Sees delivery charge calculated | "Free delivery" if subtotal > £50; else standard charge |
| 7 | Clicks "Proceed to Checkout" | Navigates to `/checkout` |

### 5b: The Checkout Flow

| Step | User Action | What Happens |
|------|-------------|-------------|
| 1 | Checkout page loads | Shows saved addresses and cards (if any) |
| 2 | Selects a saved address | Address marked as selected |
| 3 | Or clicks "Add New Address" | Inline form opens |
| 4 | Selects Standard or Express delivery | Delivery charge updates in the order summary |
| 5 | Selects a saved payment card | Card selected |
| 6 | Or enters a new card | Card fields displayed; token saved for future use |
| 7 | Reviews order summary (items, subtotal, delivery, total) | All values shown clearly |
| 8 | Clicks "Place Order" | `POST /api/v1/checkout` called |
| 9 | Order created successfully | Navigates to `/orders/confirmation/<orderNumber>` |
| 10 | Confirmation page shown | Order number, status "Placed", item list, totals |
| 11 | Basket is now empty | Header basket count resets to 0 |

---

## Journey 6: Tracking an Order

**User:** Logged in customer  
**Goal:** Check the status of an order

| Step | User Action | What Happens |
|------|-------------|-------------|
| 1 | Clicks "My Orders" in header or navigates to `/orders` | Order list loads |
| 2 | Sees list of orders with status badges | e.g., "Placed", "Dispatched", "Delivered" |
| 3 | Clicks an order | Navigates to order detail page |
| 4 | Sees full order detail | Items, quantities, prices, delivery address, payment reference |
| 5 | Sees status history timeline | Each status change shown with date/time and any admin note |
| 6 | Refreshes page | Latest status shown (no stale caching) |

**Status colour indicators:**
- Placed → grey
- Confirmed → blue
- Processing → amber
- Dispatched → orange
- Delivered → green
- Cancelled → red

---

## Journey 7: Managing a Wishlist

**User:** Logged in customer  
**Goal:** Save products for later

| Step | User Action | What Happens |
|------|-------------|-------------|
| 1 | On any product detail page, clicks "Add to Wishlist" | `POST /api/v1/wishlist/items` called; button changes to "In Wishlist" |
| 2 | Navigates to `/wishlist` | All wishlist items shown as product cards |
| 3 | Clicks a product card | Goes to product detail page |
| 4 | Clicks "Remove" on wishlist page | `DELETE /api/v1/wishlist/items/{productId}` called; card disappears |
| 5 | Clicks "Add to Basket" on wishlist page | Product added to basket; customer can proceed to checkout |
| 6 | Wishlist is empty | Empty state message: "Your wishlist is empty. Start saving products you love." |

---

## Journey 8: Account and Password Management

### 8a: Managing Delivery Addresses

| Step | User Action | What Happens |
|------|-------------|-------------|
| 1 | Navigates to account settings | Address management section visible |
| 2 | Clicks "Add New Address" | Form opens |
| 3 | Fills in label, name, address lines, town, county, postcode, phone | — |
| 4 | Clicks "Save" | `POST /api/v1/addresses`; new address appears in list |
| 5 | Clicks "Edit" on an existing address | Form pre-populated; save updates it |
| 6 | Clicks "Set as Default" | `PUT /api/v1/addresses/{id}/default`; marked with default badge |
| 7 | Clicks "Delete" | Address removed; no longer available at checkout |

### 8b: Managing Saved Payment Cards

| Step | User Action | What Happens |
|------|-------------|-------------|
| 1 | Navigates to saved cards section | List of cards with last 4 digits and brand |
| 2 | Clicks "Add Card" | Card form displayed |
| 3 | Enters card details | System tokenises and saves `Last4 + Brand + Token` |
| 4 | Clicks "Set as Default" | Default card pre-selected at checkout |
| 5 | Clicks "Remove" | Card deleted from account |

### 8c: Forgot Password

| Step | User Action | What Happens |
|------|-------------|-------------|
| 1 | Clicks "Forgot Password?" on login page | Navigates to `/forgot-password` |
| 2 | Enters registered email address | `POST /api/v1/auth/forgot-password` |
| 3 | Clicks "Send Reset Link" | Email sent (or silently succeeds if email not found — prevents enumeration) |
| 4 | Opens email and clicks reset link | Navigates to `/reset-password?token=<token>` |
| 5 | Enters and confirms new password | `POST /api/v1/auth/reset-password` |
| 6 | Success message displayed | Navigates to login; old password no longer works |
| 7 | Tries to use the same link again | Error: "This reset link has already been used or has expired" |

---

## Journey 9: Admin — Managing Products

**User:** Administrator  
**Goal:** Update the product catalogue

| Step | User Action | What Happens |
|------|-------------|-------------|
| 1 | Navigates to `/admin/products` | Full product list shown in a table |
| 2 | Clicks "Create Product" | Form opens with all product fields |
| 3 | Fills in name, SKU, category, brand, price, was-price, description, images, specs | — |
| 4 | Clicks "Save" | `POST /api/v1/admin/products`; product now visible in catalogue |
| 5 | Clicks "Edit" on existing product | Form pre-filled with current data |
| 6 | Changes the price | `PUT /api/v1/admin/products/{id}` updates the product |
| 7 | Action is logged | Audit log records: Actor, "Updated product", Product ID, timestamp |

---

## Journey 10: Admin — Managing Orders

**User:** Administrator  
**Goal:** Progress an order through fulfilment

| Step | User Action | What Happens |
|------|-------------|-------------|
| 1 | Navigates to `/admin/orders` | All orders listed with status badges |
| 2 | Clicks an order | Full order detail shown |
| 3 | Clicks "Update Status" | Dropdown with valid next states appears |
| 4 | Selects "Confirmed" and adds note: "Payment verified" | `PATCH /api/v1/admin/orders/{id}/status` |
| 5 | Status updated | Status badge changes; note appears in customer's order timeline |
| 6 | Later updates to "Dispatched" | Same flow; another entry added to timeline |
| 7 | Customer views their order | Sees "Dispatched" with the timeline of all changes |

### Order Status Transition Rules

```
Placed        → Confirmed, Cancelled
Confirmed     → Processing, Cancelled
Processing    → Dispatched, Cancelled
Dispatched    → Delivered
Delivered     → (terminal — no further transitions)
Cancelled     → (terminal — no further transitions)
```

---

## Journey 11: AI Recommendations Experience

**User:** Any (personalised for logged-in users)  
**Goal:** Discover products through AI curation

| Step | Scenario | What Happens |
|------|----------|-------------|
| 1 | New visitor lands on home page | Gemini called with anonymous context; 6 products selected from pool |
| 2 | Logged-in customer with order history visits | Gemini prompt includes last 5 orders' product names; personalised picks shown |
| 3 | Customer sees "AI Picked for You" section | 6 product cards with "AI Picked for you" badge and Gemini reason |
| 4 | Customer hovers over a card | Reason text readable; "Powered by Gemini" label visible |
| 5 | Customer clicks a card | Navigates to full product detail |
| 6 | Customer visits again within 30 minutes | Same recommendations returned from cache (no API call) |
| 7 | Gemini API is down | Fallback products shown with "You might like this" reason; section remains visible |
| 8 | Complete AI failure | Section shows: "AI suggestions unavailable right now" — rest of page unaffected |

---

## Error States and Edge Cases

### Product Not Found
**Trigger:** User navigates to `/product/invalid-slug`  
**Response:** 404 page with "Product not found" message and link to browse all products

### Empty Basket at Checkout
**Trigger:** User navigates directly to `/checkout` with an empty basket  
**Response:** Redirect to `/basket` with message: "Your basket is empty"

### Session Expired Mid-Checkout
**Trigger:** JWT expires while user is on checkout page  
**Response:** `AuthInterceptor` catches the 401; redirects to login with return URL; basket is preserved

### Duplicate Wishlist Item
**Trigger:** User attempts to add an already-wishlisted product  
**Response:** API returns 409 Conflict; UI shows "Already in your wishlist" — no duplicate created

### Out-of-Stock Product in Basket
**Trigger:** Product goes out of stock after being added to basket  
**Response:** Item remains in basket but checkout may warn if stock cannot be fulfilled (future enhancement)

### Admin Unauthorised Access
**Trigger:** Non-admin user navigates to `/admin`  
**Response:** `AdminGuard` redirects to home page silently

### Email Not Sent (SMTP Down)
**Trigger:** SMTP server is unreachable during password reset request  
**Response:** Operation logged; API returns success anyway (prevents timing-based email enumeration)

---

## Interaction States Reference

Every data-bound page in the application implements four states. Here is the pattern:

| State | When | What the User Sees |
|-------|------|--------------------|
| **Loading** | Data is being fetched from the API | Skeleton placeholder cards (grey animated blocks) |
| **Success** | Data returned and rendered | Full content with images, prices, and interactions |
| **Empty** | API returned an empty list | Friendly message with guidance (e.g., "No orders yet") |
| **Error** | API call failed or returned an error | Error message with retry option or navigation link |

This guarantees users are never left looking at a blank white screen regardless of what happens in the network or backend.
