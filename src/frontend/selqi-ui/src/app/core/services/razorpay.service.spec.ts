import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { firstValueFrom } from 'rxjs';
import { RazorpayService } from './razorpay.service';
import { environment } from '../../../environments/environment';
import { CreateRazorpayOrderResponse, VerifyRazorpayPaymentRequest } from '../../shared/models/razorpay.model';
import { OrderDetail } from '../../shared/models/order.model';

const API = environment.apiBase;

describe('RazorpayService', () => {
  let service: RazorpayService;
  let http: HttpTestingController;

  const mockOrderResponse: CreateRazorpayOrderResponse = {
    razorpayOrderId: 'order_test123',
    amount: 2995,
    currency: 'INR',
    keyId: 'rzp_test_key',
    companyName: 'Selqi',
    companyLogoUrl: '',
    themeColor: '#4f46e5'
  };

  const mockOrderDetail: OrderDetail = {
    id: 'order-uuid',
    orderNumber: 'ORD-2026-12345',
    status: 'Confirmed',
    items: [],
    subTotal: 29.95,
    deliveryCharge: 3.95,
    total: 33.90,
    deliveryName: 'Alice Smith',
    deliveryAddress: '123 High St, London, SW1A 1AA',
    paymentReference: 'rzp_order_test123_pay_abc',
    placedAt: new Date().toISOString(),
    statusHistory: []
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        RazorpayService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(RazorpayService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  // ── Positive ────────────────────────────────────────────────────────────

  it('createOrder sends POST to /razorpay/create-order with deliveryOption', () => {
    service.createOrder('standard').subscribe(result => {
      expect(result.razorpayOrderId).toBe('order_test123');
    });

    const req = http.expectOne(`${API}/razorpay/create-order`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ deliveryOption: 'standard' });
    req.flush(mockOrderResponse);
  });

  it('createOrder sends express deliveryOption correctly', () => {
    service.createOrder('express').subscribe();

    const req = http.expectOne(`${API}/razorpay/create-order`);
    expect(req.request.body.deliveryOption).toBe('express');
    req.flush(mockOrderResponse);
  });

  it('verifyPayment sends POST to /razorpay/verify-payment with full request body', () => {
    const verifyRequest: VerifyRazorpayPaymentRequest = {
      razorpayOrderId: 'order_test123',
      razorpayPaymentId: 'pay_abc',
      razorpaySignature: 'sig_xyz',
      deliveryAddress: {
        firstName: 'Alice', lastName: 'Smith',
        line1: '123 High St',
        town: 'London',
        postcode: 'SW1A 1AA', phone: '+447911123456'
      },
      deliveryOption: 'standard'
    };

    service.verifyPayment(verifyRequest).subscribe(result => {
      expect(result.orderNumber).toBe('ORD-2026-12345');
    });

    const req = http.expectOne(`${API}/razorpay/verify-payment`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body.razorpayOrderId).toBe('order_test123');
    expect(req.request.body.razorpayPaymentId).toBe('pay_abc');
    req.flush(mockOrderDetail);
  });

  // ── Negative ─────────────────────────────────────────────────────────────

  it('createOrder propagates HTTP 401 when user is not authenticated', async () => {
    const promise = firstValueFrom(service.createOrder('standard')).catch(e => e);

    http.expectOne(`${API}/razorpay/create-order`).flush(
      { message: 'Unauthorized' },
      { status: 401, statusText: 'Unauthorized' }
    );

    const err = await promise;
    expect(err.status).toBe(401);
  });

  it('createOrder propagates HTTP 400 when basket is empty', async () => {
    const promise = firstValueFrom(service.createOrder('standard')).catch(e => e);

    http.expectOne(`${API}/razorpay/create-order`).flush(
      { message: 'Your basket is empty.' },
      { status: 400, statusText: 'Bad Request' }
    );

    const err = await promise;
    expect(err.status).toBe(400);
  });

  it('verifyPayment propagates HTTP 400 when signature is invalid', async () => {
    const verifyRequest: VerifyRazorpayPaymentRequest = {
      razorpayOrderId: 'order_bad',
      razorpayPaymentId: 'pay_bad',
      razorpaySignature: 'tampered_sig',
      deliveryAddress: {
        firstName: 'Eve', lastName: 'Evil',
        line1: '999 Fraud Lane',
        town: 'London',
        postcode: 'EC1A 1BB', phone: '+440000000000'
      },
      deliveryOption: 'standard'
    };

    const promise = firstValueFrom(service.verifyPayment(verifyRequest)).catch(e => e);

    http.expectOne(`${API}/razorpay/verify-payment`).flush(
      { message: 'Payment signature verification failed.' },
      { status: 400, statusText: 'Bad Request' }
    );

    const err = await promise;
    expect(err.status).toBe(400);
  });
});
