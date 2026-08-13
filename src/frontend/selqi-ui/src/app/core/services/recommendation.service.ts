import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AiSuggestion } from '../../shared/models/recommendation.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class RecommendationService {
  private readonly http = inject(HttpClient);

  getSuggestions(): Observable<AiSuggestion[]> {
    return this.http.get<AiSuggestion[]>(`${environment.apiBase}/recommendations`);
  }
}
