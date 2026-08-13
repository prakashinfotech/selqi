import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { BasketService } from '../../../core/services/basket.service';

type FieldErrors = Partial<Record<string, string>>;

const EMAIL_RE = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  private readonly auth = inject(AuthService);
  private readonly basket = inject(BasketService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  protected email = '';
  protected password = '';
  protected loading = signal(false);
  protected serverError = signal('');
  protected fieldErrors = signal<FieldErrors>({});

  protected submit(): void {
    if (!this.validate()) return;

    const raw = this.route.snapshot.queryParamMap.get('returnUrl') ?? '/';
    const returnUrl = raw.startsWith('/') ? raw : '/';

    this.serverError.set('');
    this.loading.set(true);
    this.auth.login({ email: this.email.trim().toLowerCase(), password: this.password }).subscribe({
      next: () => {
        this.basket.load().subscribe();
        this.router.navigateByUrl(returnUrl);
      },
      error: (err) => {
        this.serverError.set(err?.error?.message ?? 'Invalid email or password.');
        this.loading.set(false);
      }
    });
  }

  protected clearFieldError(field: string): void {
    const current = this.fieldErrors();
    if (current[field]) {
      const updated = { ...current };
      delete updated[field];
      this.fieldErrors.set(updated);
    }
  }

  private validate(): boolean {
    const errs: FieldErrors = {};
    if (!this.email.trim()) errs['email'] = 'Email address is required.';
    else if (!EMAIL_RE.test(this.email.trim())) errs['email'] = 'Enter a valid email address.';
    if (!this.password) errs['password'] = 'Password is required.';
    this.fieldErrors.set(errs);
    return Object.keys(errs).length === 0;
  }
}
