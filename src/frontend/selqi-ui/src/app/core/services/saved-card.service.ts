import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SavedCard, SaveCardRequest } from '../../shared/models/saved-card.model';
import { environment } from '../../../environments/environment';

const BASE = `${environment.apiBase}/saved-cards`;

@Injectable({ providedIn: 'root' })
export class SavedCardService {
  private readonly http = inject(HttpClient);

  getCards(): Observable<SavedCard[]> {
    return this.http.get<SavedCard[]>(BASE);
  }

  saveCard(req: SaveCardRequest): Observable<SavedCard> {
    return this.http.post<SavedCard>(BASE, req);
  }

  deleteCard(id: string): Observable<void> {
    return this.http.delete<void>(`${BASE}/${id}`);
  }

  setDefault(id: string): Observable<void> {
    return this.http.put<void>(`${BASE}/${id}/default`, {});
  }
}
