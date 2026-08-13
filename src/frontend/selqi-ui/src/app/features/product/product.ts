import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CatalogService } from '../../core/services/catalog.service';
import { BasketService } from '../../core/services/basket.service';
import { OrderService } from '../../core/services/order.service';
import { AuthService } from '../../core/services/auth.service';
import { ProductDetail } from '../../shared/models/product.model';

@Component({
  selector: 'app-product',
  imports: [RouterLink],
  templateUrl: './product.html',
  styleUrl: './product.scss'
})
export class Product implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly catalogSvc = inject(CatalogService);
  private readonly basketSvc = inject(BasketService);
  private readonly orderSvc = inject(OrderService);
  protected readonly auth = inject(AuthService);

  protected readonly product = signal<ProductDetail | null>(null);
  protected readonly loading = signal(true);
  protected readonly selectedImage = signal(0);
  protected readonly quantity = signal(1);
  protected readonly addingToBasket = signal(false);
  protected readonly addedToBasket = signal(false);
  protected readonly inWishlist = signal(false);
  protected readonly wishlistLoading = signal(false);
  protected readonly activeTab = signal<'specs' | 'delivery'>('specs');

  protected readonly currentImage = computed(() => {
    const p = this.product();
    if (!p || p.images.length === 0) return null;
    return p.images[this.selectedImage()] ?? p.images[0];
  });

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug')!;
      this.loadProduct(slug);
    });
    if (this.auth.isAuthenticated()) {
      this.loadWishlist();
    }
  }

  private loadProduct(slug: string): void {
    this.loading.set(true);
    this.catalogSvc.getProductBySlug(slug).subscribe({
      next: p => { this.product.set(p); this.loading.set(false); },
      error: () => { this.loading.set(false); }
    });
  }

  private loadWishlist(): void {
    this.orderSvc.getWishlist().subscribe({
      next: wishlist => {
        const productId = this.product()?.id;
        if (productId) {
          this.inWishlist.set(wishlist.items.some(i => i.productId === productId));
        }
      }
    });
  }

  protected selectImage(index: number): void {
    this.selectedImage.set(index);
  }

  protected adjustQty(delta: number): void {
    this.quantity.update(q => Math.max(1, Math.min(99, q + delta)));
  }

  protected addToBasket(): void {
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: this.router.url } });
      return;
    }
    const product = this.product();
    if (!product) return;
    this.addingToBasket.set(true);
    this.basketSvc.addItem({ productId: product.id, quantity: this.quantity() }).subscribe({
      next: () => {
        this.addingToBasket.set(false);
        this.addedToBasket.set(true);
        setTimeout(() => this.addedToBasket.set(false), 2500);
      },
      error: () => this.addingToBasket.set(false)
    });
  }

  protected toggleWishlist(): void {
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: this.router.url } });
      return;
    }
    const product = this.product();
    if (!product) return;
    this.wishlistLoading.set(true);
    const action = this.inWishlist()
      ? this.orderSvc.removeFromWishlist(product.id)
      : this.orderSvc.addToWishlist(product.id);
    action.subscribe({
      next: () => {
        this.inWishlist.update(v => !v);
        this.wishlistLoading.set(false);
      },
      error: () => this.wishlistLoading.set(false)
    });
  }
}
