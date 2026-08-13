import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';

const EMAIL_RE = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;

@Component({
  selector: 'app-forgot-password',
  imports: [FormsModule, RouterLink],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.scss'
})
export class ForgotPassword {
  private readonly auth = inject(AuthService);

  protected email = '';
  protected loading = signal(false);
  protected submitted = signal(false);
  protected emailError = signal('');

  protected submit(): void {
    const trimmed = this.email.trim();
    if (!trimmed) { this.emailError.set('Email address is required.'); return; }
    if (!EMAIL_RE.test(trimmed)) { this.emailError.set('Enter a valid email address.'); return; }

    this.emailError.set('');
    this.loading.set(true);
    this.auth.forgotPassword(trimmed.toLowerCase()).subscribe({
      next: () => { this.loading.set(false); this.submitted.set(true); },
      error: () => {
        // Always show success — backend never reveals whether email exists
        this.loading.set(false);
        this.submitted.set(true);
      }
    });
  }
}
