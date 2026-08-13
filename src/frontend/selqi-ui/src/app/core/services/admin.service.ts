import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import {
  AdminDashboard,
  AdminOrder,
  AdminProduct,
  AdminStore,
  AdminStoreDetail,
  AuditLog,
  CreateProductRequest,
  CreateStoreRequest,
  UpdateAdminProductRequest,
  UpdateStoreInventoryRequest,
  UpdateStoreRequest
} from '../../shared/models/admin.model';

const API_BASE = environment.apiBase;

@Injectable({ providedIn: 'root' })
export class AdminService {
  private readonly http = inject(HttpClient);

  getCategories() {
    return this.http.get<{ id: string; name: string }[]>(`${API_BASE}/categories`);
  }

  getBrands() {
    return this.http.get<{ id: string; name: string }[]>(`${API_BASE}/brands`);
  }

  getDashboard() {
    return this.http.get<AdminDashboard>(`${API_BASE}/admin/dashboard`);
  }

  getProducts() {
    return this.http.get<AdminProduct[]>(`${API_BASE}/admin/products`);
  }

  createProduct(request: CreateProductRequest) {
    return this.http.post<AdminProduct>(`${API_BASE}/admin/products`, request);
  }

  updateProduct(productId: string, request: UpdateAdminProductRequest) {
    return this.http.put<AdminProduct>(`${API_BASE}/admin/products/${productId}`, request);
  }

  getOrders() {
    return this.http.get<AdminOrder[]>(`${API_BASE}/admin/orders`);
  }

  updateOrderStatus(orderId: string, status: string) {
    return this.http.patch<AdminOrder>(`${API_BASE}/admin/orders/${orderId}/status`, { status });
  }

  getAuditLogs() {
    return this.http.get<AuditLog[]>(`${API_BASE}/admin/audit-logs`);
  }

  getStores() {
    return this.http.get<AdminStore[]>(`${API_BASE}/admin/stores`);
  }

  getStore(storeId: string) {
    return this.http.get<AdminStoreDetail>(`${API_BASE}/admin/stores/${storeId}`);
  }

  createStore(request: CreateStoreRequest) {
    return this.http.post<AdminStoreDetail>(`${API_BASE}/admin/stores`, request);
  }

  updateStore(storeId: string, request: UpdateStoreRequest) {
    return this.http.put<AdminStoreDetail>(`${API_BASE}/admin/stores/${storeId}`, request);
  }

  updateStoreInventory(storeId: string, request: UpdateStoreInventoryRequest) {
    return this.http.patch<void>(`${API_BASE}/admin/stores/${storeId}/inventory`, request);
  }

  deleteStore(storeId: string) {
    return this.http.delete<void>(`${API_BASE}/admin/stores/${storeId}`);
  }
}
