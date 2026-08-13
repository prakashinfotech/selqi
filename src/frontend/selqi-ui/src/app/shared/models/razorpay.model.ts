export interface CreateRazorpayOrderResponse {
  razorpayOrderId: string;
  amount: number;         // smallest unit (paise / pence)
  currency: string;
  keyId: string;
  companyName: string;
  companyLogoUrl: string;
  themeColor: string;
}

export interface VerifyRazorpayPaymentRequest {
  razorpayOrderId: string;
  razorpayPaymentId: string;
  razorpaySignature: string;
  deliveryAddress: {
    firstName: string;
    lastName: string;
    line1: string;
    line2?: string;
    town: string;
    county?: string;
    postcode: string;
    phone: string;
  };
  deliveryOption: string;
}

// Shape of the success response from the Razorpay modal handler callback
export interface RazorpayHandlerResponse {
  razorpay_order_id: string;
  razorpay_payment_id: string;
  razorpay_signature: string;
}
