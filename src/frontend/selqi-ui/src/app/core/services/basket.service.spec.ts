import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { BasketService } from './basket.service';
import { environment } from '../../../environments/environment';
import { Basket } from '../../shared/models/basket.model';

describe('BasketService', () => {
  let service: BasketService;
  let http: HttpTestingController;

  const basket: Basket = {
    id: 'basket-1',
    itemCount: 1,
    subTotal: 19.99,
    items: [
      {
        id: 'item-1',
        productId: 'product-1',
        productName: 'Air fryer',
        productSlug: 'air-fryer',
        productImageUrl: null,
        unitPrice: 19.99,
        wasPrice: null,
        quantity: 1,
        lineTotal: 19.99
      }
    ]
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        BasketService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(BasketService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    http.verify();
  });

  it('loads the current basket into signal state', () => {
    service.load().subscribe(result => {
      expect(result.itemCount).toBe(1);
    });

    http.expectOne(`${environment.apiBase}/basket`).flush(basket);

    expect(service.basket()?.id).toBe('basket-1');
  });

  it('posts add item request and stores returned basket', () => {
    service.addItem({ productId: 'product-1', quantity: 2 }).subscribe();

    const request = http.expectOne(`${environment.apiBase}/basket/items`);
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual({ productId: 'product-1', quantity: 2 });
    request.flush({ ...basket, itemCount: 2 });

    expect(service.basket()?.itemCount).toBe(2);
  });

  it('clears local basket state after clear call succeeds', () => {
    service.load().subscribe();
    http.expectOne(`${environment.apiBase}/basket`).flush(basket);
    expect(service.basket()).not.toBeNull();

    service.clear().subscribe();
    http.expectOne(`${environment.apiBase}/basket`).flush({});

    expect(service.basket()).toBeNull();
  });
});
