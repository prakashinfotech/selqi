import { Component, inject, signal, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { OrderService } from '../../core/services/order.service';
import { BasketService } from '../../core/services/basket.service';
import { WishlistItem } from '../../shared/models/order.model';

@Component({
  selector: 'app-wishlist',
  imports: [RouterLink],
  templateUrl: './wishlist.html',
  styleUrl: './wishlist.scss'
})
export class WishlistPage implements OnInit {
  private readonly orderSvc = inject(OrderService);
  private readonly basketSvc = inject(BasketService);

  protected readonly items = signal<WishlistItem[]>([]);
  protected readonly loading = signal(true);
  protected readonly addingToBasket = signal<string | null>(null);

  ngOnInit(): void {
    this.orderSvc.getWishlist().subscribe({
      next: w => { this.items.set(w.items); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  protected removeItem(productId: string): void {
    this.orderSvc.removeFromWishlist(productId).subscribe({
      next: w => this.items.set(w.items)
    });
  }

  protected addToBasket(item: WishlistItem): void {
    this.addingToBasket.set(item.productId);
    this.basketSvc.addItem({ productId: item.productId, quantity: 1 }).subscribe({
      next: () => this.addingToBasket.set(null),
      error: () => this.addingToBasket.set(null)
    });
  }
}
