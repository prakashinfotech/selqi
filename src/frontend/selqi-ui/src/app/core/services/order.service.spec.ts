import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { firstValueFrom } from 'rxjs';
import { OrderService } from './order.service';
import { environment } from '../../../environments/environment';
import { CheckoutRequest, OrderDetail, OrderSummary } from '../../shared/models/order.model';

const API = environment.apiBase;

describe('OrderService', () => {
  let service: OrderService;
  let http: HttpTestingController;

  const mockOrderDetail: OrderDetail = {
    id: 'order-uuid',
    orderNumber: 'ORD-2026-12345',
    status: 'Pending',
    items: [
      {
        productId: 'prod-1',
        productName: 'Smart TV',
        productSku: 'SKU-TV1',
        productImageUrl: null,
        quantity: 1,
        unitPrice: 299.99,
        lineTotal: 299.99
      }
    ],
    subTotal: 299.99,
    deliveryCharge: 3.95,
    total: 303.94,
    deliveryName: 'Alice Smith',
    deliveryAddress: '123 High St, London, SW1A 1AA',
    paymentReference: 'mock_card_abc123',
    placedAt: new Date().toISOString(),
    statusHistory: [{ status: 'Pending', changedAt: new Date().toISOString(), note: 'Order placed' }]
  };

  const mockCheckoutRequest: CheckoutRequest = {
    deliveryAddress: {
      firstName: 'Alice', lastName: 'Smith',
      line1: '123 High St',
      town: 'London',
      postcode: 'SW1A 1AA', phone: '+447911123456'
    },
    deliveryOption: 'standard',
    payment: {
      cardNumber: '1111',
      cardHolder: 'Alice Smith',
      expiryMonth: '12',
      expiryYear: '27',
      saveCard: false,
      cardBrand: 'Visa'
    }
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        OrderService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(OrderService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  // ── Positive ────────────────────────────────────────────────────────────

  it('placeOrder sends POST to /checkout with checkout request', () => {
    service.placeOrder(mockCheckoutRequest).subscribe(result => {
      expect(result.orderNumber).toBe('ORD-2026-12345');
    });

    const req = http.expectOne(`${API}/checkout`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body.deliveryOption).toBe('standard');
    req.flush(mockOrderDetail);
  });

  it('getOrders sends GET to /orders and returns list', () => {
    const mockSummaries: OrderSummary[] = [
      { id: 'order-uuid', orderNumber: 'ORD-2026-12345', status: 'Pending',
        itemCount: 1, total: 303.94, placedAt: new Date().toISOString() }
    ];

    service.getOrders().subscribe(orders => {
      expect(orders).toHaveLength(1);
      expect(orders[0].orderNumber).toBe('ORD-2026-12345');
    });

    const req = http.expectOne(`${API}/orders`);
    expect(req.request.method).toBe('GET');
    req.flush(mockSummaries);
  });

  it('getOrder sends GET to /orders/:orderNumber and returns detail', () => {
    service.getOrder('ORD-2026-12345').subscribe(order => {
      expect(order.status).toBe('Pending');
      expect(order.items).toHaveLength(1);
    });

    const req = http.expectOne(`${API}/orders/ORD-2026-12345`);
    expect(req.request.method).toBe('GET');
    req.flush(mockOrderDetail);
  });

  it('addToWishlist sends POST to /wishlist/items', () => {
    service.addToWishlist('prod-1').subscribe();

    const req = http.expectOne(`${API}/wishlist/items`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ productId: 'prod-1' });
    req.flush({ items: [] });
  });

  it('removeFromWishlist sends DELETE to /wishlist/items/:productId', () => {
    service.removeFromWishlist('prod-1').subscribe();

    const req = http.expectOne(`${API}/wishlist/items/prod-1`);
    expect(req.request.method).toBe('DELETE');
    req.flush({ items: [] });
  });

  // ── Negative ─────────────────────────────────────────────────────────────

  it('placeOrder propagates HTTP 400 when basket is empty', async () => {
    const promise = firstValueFrom(service.placeOrder(mockCheckoutRequest)).catch(e => e);

    http.expectOne(`${API}/checkout`).flush(
      { message: 'Your basket is empty.' },
      { status: 400, statusText: 'Bad Request' }
    );

    const err = await promise;
    expect(err.status).toBe(400);
  });

  it('placeOrder propagates HTTP 401 when user is not authenticated', async () => {
    const promise = firstValueFrom(service.placeOrder(mockCheckoutRequest)).catch(e => e);

    http.expectOne(`${API}/checkout`).flush(
      { message: 'Unauthorized' },
      { status: 401, statusText: 'Unauthorized' }
    );

    const err = await promise;
    expect(err.status).toBe(401);
  });

  it('getOrder propagates HTTP 404 when order does not exist', async () => {
    const promise = firstValueFrom(service.getOrder('ORD-0000-00000')).catch(e => e);

    http.expectOne(`${API}/orders/ORD-0000-00000`).flush(
      { message: 'Order not found.' },
      { status: 404, statusText: 'Not Found' }
    );

    const err = await promise;
    expect(err.status).toBe(404);
  });

  it('getOrder propagates HTTP 403 when order belongs to another user', async () => {
    const promise = firstValueFrom(service.getOrder('ORD-2026-99999')).catch(e => e);

    http.expectOne(`${API}/orders/ORD-2026-99999`).flush(
      { message: 'Access denied.' },
      { status: 403, statusText: 'Forbidden' }
    );

    const err = await promise;
    expect(err.status).toBe(403);
  });
});
