# Payment Integration

Selqi currently supports a local demonstration card path and Razorpay sandbox. Neither path should be presented as production-ready payment processing.

## Razorpay Configuration

Store sandbox values outside tracked configuration:

```powershell
dotnet user-secrets set "Razorpay:KeyId" "<sandbox-key-id>" --project src/backend/Selqi.Api
dotnet user-secrets set "Razorpay:KeySecret" "<sandbox-key-secret>" --project src/backend/Selqi.Api
```

The configured currency is INR. The application converts GBP order totals with `Razorpay:GbpToInrRate`; production systems should use an approved pricing/FX source instead of a static development value.

## Razorpay Flow

1. The authenticated frontend requests `POST /api/v1/razorpay/create-order`.
2. The API validates the checkout request and creates a Razorpay order server-side.
3. The browser opens Razorpay Checkout using the public key ID and returned order ID.
4. After provider success, the frontend sends the payment/order identifiers and signature to `POST /api/v1/razorpay/verify-payment`.
5. The server verifies the HMAC signature before it completes local checkout and creates the order.

## Local Card Flow

The direct checkout path is for demonstration and automated testing. It records only the payment method and order result. Saved-card records contain provider tokens plus display metadata such as brand, expiry, and last four digits. Raw PAN and CVV must not be persisted or logged.

## Production Checklist

- Replace sandbox keys with secret-store references and rotate exposed test keys.
- Recalculate totals and stock server-side; never trust client amounts or success callbacks.
- Add idempotency for payment verification and order creation.
- Define webhook verification, refund, reconciliation, timeout, and failure-handling procedures.
- Complete the payment provider's production onboarding and applicable PCI/security review.
