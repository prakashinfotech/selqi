import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SavedAddress, SaveAddressRequest } from '../../shared/models/auth.model';
import { environment } from '../../../environments/environment';

const API_BASE = environment.apiBase;

@Injectable({ providedIn: 'root' })
export class AddressService {
  private readonly http = inject(HttpClient);

  getAddresses() {
    return this.http.get<SavedAddress[]>(`${API_BASE}/addresses`);
  }

  saveAddress(request: SaveAddressRequest) {
    return this.http.post<SavedAddress>(`${API_BASE}/addresses`, request);
  }

  deleteAddress(id: string) {
    return this.http.delete(`${API_BASE}/addresses/${id}`);
  }

  setDefault(id: string) {
    return this.http.put(`${API_BASE}/addresses/${id}/default`, {});
  }
}
