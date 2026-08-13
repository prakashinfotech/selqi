import { Component, inject, signal, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';

type FieldErrors = Partial<Record<string, string>>;

@Component({
  selector: 'app-reset-password',
  imports: [FormsModule, RouterLink],
  templateUrl: './reset-password.html',
  styleUrl: './reset-password.scss'
})
export class ResetPassword implements OnInit {
  private readonly auth = inject(AuthService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  protected token = '';
  protected newPassword = '';
  protected confirmPassword = '';
  protected loading = signal(false);
  protected success = signal(false);
  protected serverError = signal('');
  protected fieldErrors = signal<FieldErrors>({});

  ngOnInit(): void {
    this.token = this.route.snapshot.queryParamMap.get('token') ?? '';
    if (!this.token) {
      this.serverError.set('Invalid or missing reset token. Please request a new password reset link.');
    }
  }

  protected submit(): void {
    if (!this.validate()) return;

    this.serverError.set('');
    this.loading.set(true);
    this.auth.resetPassword(this.token, this.newPassword, this.confirmPassword).subscribe({
      next: () => {
        this.loading.set(false);
        this.success.set(true);
        setTimeout(() => this.router.navigate(['/login']), 3000);
      },
      error: (err) => {
        this.serverError.set(err?.error?.message ?? 'Something went wrong. Please try again.');
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

    if (!this.newPassword) errs['newPassword'] = 'New password is required.';
    else if (this.newPassword.length < 8) errs['newPassword'] = 'Password must be at least 8 characters.';
    else if (this.newPassword.length > 100) errs['newPassword'] = 'Password must be 100 characters or less.';

    if (!this.confirmPassword) errs['confirmPassword'] = 'Please confirm your new password.';
    else if (this.newPassword && this.confirmPassword !== this.newPassword) errs['confirmPassword'] = 'Passwords do not match.';

    this.fieldErrors.set(errs);
    return Object.keys(errs).length === 0;
  }
}
