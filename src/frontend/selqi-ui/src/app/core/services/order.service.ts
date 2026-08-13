import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CheckoutRequest, OrderDetail, OrderSummary } from '../../shared/models/order.model';
import { Wishlist } from '../../shared/models/order.model';
import { environment } from '../../../environments/environment';

const API_BASE = environment.apiBase;

@Injectable({ providedIn: 'root' })
export class OrderService {
  private readonly http = inject(HttpClient);

  placeOrder(request: CheckoutRequest) {
    return this.http.post<OrderDetail>(`${API_BASE}/checkout`, request);
  }

  getOrders() {
    return this.http.get<OrderSummary[]>(`${API_BASE}/orders`);
  }

  getOrder(orderNumber: string) {
    return this.http.get<OrderDetail>(`${API_BASE}/orders/${orderNumber}`);
  }

  getWishlist() {
    return this.http.get<Wishlist>(`${API_BASE}/wishlist`);
  }

  addToWishlist(productId: string) {
    return this.http.post<Wishlist>(`${API_BASE}/wishlist/items`, { productId });
  }

  removeFromWishlist(productId: string) {
    return this.http.delete<Wishlist>(`${API_BASE}/wishlist/items/${productId}`);
  }
}
