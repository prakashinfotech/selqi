import { Component, inject, OnInit, computed, signal, HostListener, ElementRef } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CatalogService } from '../../../core/services/catalog.service';
import { AuthService } from '../../../core/services/auth.service';
import { BasketService } from '../../../core/services/basket.service';

type Panel = 'shop' | 'trending' | 'returns' | 'help' | null;

const TRENDING_SECTIONS: Array<{ heading: string; links: Array<{ label: string; route: string; params: Record<string, string> }> }> = [
  {
    heading: 'What\'s Hot',
    links: [
      { label: 'New arrivals',    route: '/products', params: { sort: 'new' } },
      { label: 'Best sellers',    route: '/products', params: { sort: 'rating' } },
      { label: 'Deals & offers',  route: '/products', params: { offersOnly: 'true' } },
      { label: 'Featured picks',  route: '/products', params: { featured: 'true' } },
    ]
  },
  {
    heading: 'Top Categories',
    links: [
      { label: 'Gaming',          route: '/products', params: { search: 'gaming' } },
      { label: 'Smart Home',      route: '/products', params: { search: 'smart home' } },
      { label: 'Garden & Outdoors', route: '/products', params: { search: 'garden' } },
      { label: 'Baby & Nursery',  route: '/products', params: { search: 'baby' } },
    ]
  },
  {
    heading: 'Shop by Need',
    links: [
      { label: 'Gifts under £25', route: '/products', params: { maxPrice: '25', sort: 'rating' } },
      { label: 'Gifts under £50', route: '/products', params: { maxPrice: '50', sort: 'rating' } },
      { label: 'Student essentials', route: '/products', params: { search: 'student' } },
      { label: 'Back to school',  route: '/products', params: { search: 'school' } },
    ]
  }
];

const RETURNS_INFO = [
  { q: 'How do I return an item?',      a: 'Bring your item and proof of purchase to any Selqi store within 30 days. Items must be unused and in original packaging.' },
  { q: 'What is the returns window?',   a: 'Most items can be returned within 30 days. Large items (beds, sofas) have a 14-day window. Personalised items cannot be returned.' },
  { q: 'Can I return online orders?',   a: 'Yes – return to any store or arrange a collection via our Returns portal. Fast-track collection is available Monday–Saturday.' },
  { q: 'What about faulty items?',      a: 'Faulty items are covered under our 12-month guarantee. Contact Help & Support and we\'ll arrange a repair, replacement, or refund.' },
];

const HELP_ITEMS = [
  { q: 'How do I track my order?',      a: 'Sign in and go to My Orders to see live status updates. You\'ll also receive email notifications at each stage.' },
  { q: 'When will my order arrive?',    a: 'Standard delivery: 3–5 working days. Express delivery: next day if ordered before 6 pm. Check your confirmation email for your estimated date.' },
  { q: 'Can I change or cancel?',       a: 'Orders can be cancelled within 30 minutes of placement via My Orders. After that, contact our support team.' },
  { q: 'How do I use Selqi Pay Credit?', a: 'Select "Selqi Pay" at checkout. Subject to status. Representative 34.9% APR variable. Only available on orders over £99.' },
];

@Component({
  selector: 'app-header',
  imports: [RouterLink],
  templateUrl: './header.html',
  styleUrl: './header.scss'
})
export class Header implements OnInit {
  protected readonly catalog = inject(CatalogService);
  protected readonly auth = inject(AuthService);
  protected readonly basketSvc = inject(BasketService);
  private readonly router = inject(Router);
  private readonly el = inject(ElementRef);

  protected readonly basketCount = computed(() =>
    this.basketSvc.basket()?.itemCount ?? 0
  );

  // Active panel (shop, trending, returns, help, or null)
  protected readonly activePanel = signal<Panel>(null);

  // Expanded FAQ items (index → open)
  protected readonly openFaq = signal<Set<number>>(new Set());

  protected readonly trendingSections = TRENDING_SECTIONS;
  protected readonly returnsInfo = RETURNS_INFO;
  protected readonly helpItems = HELP_ITEMS;

  // Top-level (parent) categories only, sorted by displayOrder
  protected readonly topCategories = computed(() =>
    this.catalog.categories()
      .filter(c => !c.parentCategoryId)
      .sort((a, b) => a.displayOrder - b.displayOrder)
  );

  ngOnInit(): void {
    this.catalog.loadCategories();
    if (this.auth.isAuthenticated()) {
      this.basketSvc.load().subscribe();
    }
  }

  protected togglePanel(panel: Panel): void {
    this.activePanel.set(this.activePanel() === panel ? null : panel);
    this.openFaq.set(new Set());
  }

  protected toggleFaq(index: number): void {
    const cur = new Set(this.openFaq());
    cur.has(index) ? cur.delete(index) : cur.add(index);
    this.openFaq.set(cur);
  }

  protected isFaqOpen(index: number): boolean {
    return this.openFaq().has(index);
  }

  protected onSearch(event: Event): void {
    event.preventDefault();
    const form = event.target as HTMLFormElement;
    const searchInput = form.querySelector<HTMLInputElement>('#site-search');
    const search = searchInput?.value.trim();
    this.activePanel.set(null);
    this.router.navigate(['/products'], {
      queryParams: search ? { search } : {}
    });
  }

  protected navigateTo(route: string, params?: Record<string, string>): void {
    this.activePanel.set(null);
    this.router.navigate([route], { queryParams: params ?? {} });
  }

  protected logout(): void {
    this.auth.logout();
    this.basketSvc.reset();
  }

  @HostListener('document:keydown.escape')
  protected onEscape(): void {
    this.activePanel.set(null);
  }

  @HostListener('document:click', ['$event'])
  protected onDocumentClick(event: MouseEvent): void {
    if (!this.el.nativeElement.contains(event.target as Node)) {
      this.activePanel.set(null);
    }
  }
}
