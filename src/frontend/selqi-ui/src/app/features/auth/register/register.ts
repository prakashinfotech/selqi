import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';

type FieldErrors = Partial<Record<string, string>>;

const EMAIL_RE = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;

@Component({
  selector: 'app-register',
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss'
})
export class Register {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  protected firstName = '';
  protected lastName = '';
  protected email = '';
  protected password = '';
  protected confirmPassword = '';
  protected loading = signal(false);
  protected serverError = signal('');
  protected fieldErrors = signal<FieldErrors>({});

  protected submit(): void {
    if (!this.validate()) return;

    this.serverError.set('');
    this.loading.set(true);
    this.auth.register({
      firstName: this.firstName.trim(),
      lastName: this.lastName.trim(),
      email: this.email.trim().toLowerCase(),
      password: this.password
    }).subscribe({
      next: () => this.router.navigate(['/']),
      error: (err) => {
        this.serverError.set(err?.error?.message ?? 'Registration failed. Please try again.');
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

    if (!this.firstName.trim()) errs['firstName'] = 'First name is required.';
    else if (this.firstName.trim().length < 2) errs['firstName'] = 'Must be at least 2 characters.';
    else if (this.firstName.trim().length > 100) errs['firstName'] = 'Must be 100 characters or less.';

    if (!this.lastName.trim()) errs['lastName'] = 'Last name is required.';
    else if (this.lastName.trim().length < 2) errs['lastName'] = 'Must be at least 2 characters.';
    else if (this.lastName.trim().length > 100) errs['lastName'] = 'Must be 100 characters or less.';

    if (!this.email.trim()) errs['email'] = 'Email address is required.';
    else if (!EMAIL_RE.test(this.email.trim())) errs['email'] = 'Enter a valid email address.';
    else if (this.email.trim().length > 256) errs['email'] = 'Email must be 256 characters or less.';

    if (!this.password) errs['password'] = 'Password is required.';
    else if (this.password.length < 8) errs['password'] = 'Password must be at least 8 characters.';
    else if (this.password.length > 100) errs['password'] = 'Password must be 100 characters or less.';

    if (!this.confirmPassword) errs['confirmPassword'] = 'Please confirm your password.';
    else if (this.password && this.confirmPassword !== this.password) errs['confirmPassword'] = 'Passwords do not match.';

    this.fieldErrors.set(errs);
    return Object.keys(errs).length === 0;
  }
}
