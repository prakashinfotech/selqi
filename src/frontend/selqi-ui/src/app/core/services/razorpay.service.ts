import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateRazorpayOrderResponse, VerifyRazorpayPaymentRequest } from '../../shared/models/razorpay.model';
import { OrderDetail } from '../../shared/models/order.model';

const API_BASE = environment.apiBase;

// Global Razorpay constructor loaded via <script> in index.html
declare var Razorpay: new (options: RazorpayOptions) => RazorpayInstance;

interface RazorpayOptions {
  key: string;
  amount: number;
  currency: string;
  name: string;
  description?: string;
  image?: string;
  order_id: string;
  handler: (response: { razorpay_order_id: string; razorpay_payment_id: string; razorpay_signature: string }) => void;
  prefill?: { name?: string; contact?: string };
  theme?: { color?: string };
  modal?: { ondismiss?: () => void };
}

interface RazorpayInstance {
  open(): void;
  on(event: string, handler: () => void): void;
}

@Injectable({ providedIn: 'root' })
export class RazorpayService {
  private readonly http = inject(HttpClient);

  createOrder(deliveryOption: string): Observable<CreateRazorpayOrderResponse> {
    return this.http.post<CreateRazorpayOrderResponse>(
      `${API_BASE}/razorpay/create-order`,
      { deliveryOption }
    );
  }

  verifyPayment(request: VerifyRazorpayPaymentRequest): Observable<OrderDetail> {
    return this.http.post<OrderDetail>(`${API_BASE}/razorpay/verify-payment`, request);
  }

  openCheckout(
    order: CreateRazorpayOrderResponse,
    prefillName: string,
    prefillPhone: string,
    onSuccess: (response: { razorpay_order_id: string; razorpay_payment_id: string; razorpay_signature: string }) => void,
    onDismiss: () => void
  ): void {
    const rzp = new Razorpay({
      key: order.keyId,
      amount: order.amount,
      currency: order.currency,
      name: order.companyName || 'Selqi',
      description: 'Order Payment',
      image: order.companyLogoUrl || undefined,
      order_id: order.razorpayOrderId,
      handler: onSuccess,
      prefill: {
        name: prefillName,
        contact: prefillPhone
      },
      theme: { color: order.themeColor || '#4f46e5' },
      modal: { ondismiss: onDismiss }
    });
    rzp.open();
  }
}
