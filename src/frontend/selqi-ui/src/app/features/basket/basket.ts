import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { BasketService } from '../../core/services/basket.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-basket',
  imports: [RouterLink],
  templateUrl: './basket.html',
  styleUrl: './basket.scss'
})
export class Basket implements OnInit {
  private readonly router = inject(Router);
  protected readonly basketSvc = inject(BasketService);
  protected readonly auth = inject(AuthService);
  protected readonly loading = signal(false);

  protected readonly basket = this.basketSvc.basket;
  protected readonly deliveryCost = computed(() => {
    const sub = this.basket()?.subTotal ?? 0;
    return sub >= 50 ? 0 : 3.95;
  });
  protected readonly total = computed(() =>
    (this.basket()?.subTotal ?? 0) + this.deliveryCost()
  );

  ngOnInit(): void {
    if (this.auth.isAuthenticated()) {
      this.basketSvc.load().subscribe();
    }
  }

  protected updateQty(itemId: string, qty: number): void {
    if (qty < 1) { this.removeItem(itemId); return; }
    this.basketSvc.updateItem(itemId, { quantity: qty }).subscribe();
  }

  protected removeItem(itemId: string): void {
    this.basketSvc.removeItem(itemId).subscribe();
  }

  protected checkout(): void {
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: '/checkout' } });
      return;
    }
    this.router.navigate(['/checkout']);
  }
}
