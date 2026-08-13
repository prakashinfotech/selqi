import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { AuthResponse, LoginRequest, RegisterRequest, UserDto } from '../../shared/models/auth.model';
import { environment } from '../../../environments/environment';

const API_BASE = environment.apiBase;
const TOKEN_KEY = 'selqi_token';
const USER_KEY = 'selqi_user';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  private readonly _user = signal<UserDto | null>(this.loadUser());

  readonly currentUser = this._user.asReadonly();
  readonly isAuthenticated = computed(() => this._user() !== null);
  readonly isAdmin = computed(() => this._user()?.role === 'Admin');

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  register(request: RegisterRequest) {
    return this.http.post<AuthResponse>(`${API_BASE}/auth/register`, request).pipe(
      tap(res => this.storeSession(res))
    );
  }

  login(request: LoginRequest) {
    return this.http.post<AuthResponse>(`${API_BASE}/auth/login`, request).pipe(
      tap(res => this.storeSession(res))
    );
  }

  forgotPassword(email: string) {
    return this.http.post<{ message: string }>(`${API_BASE}/auth/forgot-password`, { email });
  }

  resetPassword(token: string, newPassword: string, confirmPassword: string) {
    return this.http.post<{ message: string }>(`${API_BASE}/auth/reset-password`, { token, newPassword, confirmPassword });
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    this._user.set(null);
    this.router.navigate(['/']);
  }

  private storeSession(res: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, res.token);
    localStorage.setItem(USER_KEY, JSON.stringify(res.user));
    this._user.set(res.user);
  }

  private loadUser(): UserDto | null {
    try {
      const raw = localStorage.getItem(USER_KEY);
      return raw ? JSON.parse(raw) : null;
    } catch {
      return null;
    }
  }
}
