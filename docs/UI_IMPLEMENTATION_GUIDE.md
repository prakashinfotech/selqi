# UI Implementation Guide

## Angular Structure

Use feature-first organization:

```text
src/app/
  core/
  shared/
  features/
    home/
    catalog/
    product/
    basket/
    checkout/
    account/
    wishlist/
    orders/
    stores/
    admin/
```

## Required Pages

- Home
- Category listing
- Product listing
- Product detail
- Basket/trolley
- Login/register
- Account profile
- Wishlist
- Address book
- Checkout
- Order confirmation
- Order history
- Track order
- Store finder
- Admin dashboard
- Admin product editor
- Admin order management

## UI Behavior Rules

- Every API page must handle loading, empty, error, and success states.
- Product cards must show image, title, price, rating placeholder, availability, and action buttons.
- Search should update route query parameters.
- Filters should be shareable through route query parameters.
- Basket should persist for signed-in users through backend APIs.
- Checkout should block order submission until address, delivery/collection, and payment are valid.

## Styling Direction

Build an original ecommerce UI inspired by retail patterns:

- Clear top navigation.
- Prominent search.
- Category menu.
- Product grid.
- Strong call-to-action buttons.
- Responsive mobile layout.
- Accessible colors and focus states.

Do not copy third-party retailer logos, images, exact page text, or proprietary artwork.

