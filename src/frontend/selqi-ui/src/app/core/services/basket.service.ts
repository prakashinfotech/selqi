import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { Basket, AddToBasketRequest, UpdateBasketItemRequest } from '../../shared/models/basket.model';
import { environment } from '../../../environments/environment';

const API_BASE = environment.apiBase;

@Injectable({ providedIn: 'root' })
export class BasketService {
  private readonly http = inject(HttpClient);

  private readonly _basket = signal<Basket | null>(null);
  readonly basket = this._basket.asReadonly();

  load() {
    return this.http.get<Basket>(`${API_BASE}/basket`).pipe(
      tap(b => this._basket.set(b))
    );
  }

  addItem(request: AddToBasketRequest) {
    return this.http.post<Basket>(`${API_BASE}/basket/items`, request).pipe(
      tap(b => this._basket.set(b))
    );
  }

  updateItem(itemId: string, request: UpdateBasketItemRequest) {
    return this.http.put<Basket>(`${API_BASE}/basket/items/${itemId}`, request).pipe(
      tap(b => this._basket.set(b))
    );
  }

  removeItem(itemId: string) {
    return this.http.delete<Basket>(`${API_BASE}/basket/items/${itemId}`).pipe(
      tap(b => this._basket.set(b))
    );
  }

  clear() {
    return this.http.delete(`${API_BASE}/basket`).pipe(
      tap(() => this._basket.set(null))
    );
  }

  reset(): void {
    this._basket.set(null);
  }
}
