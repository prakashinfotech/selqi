import { Component, HostListener, inject, signal, computed, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { combineLatest } from 'rxjs';
import { CatalogService } from '../../core/services/catalog.service';
import { BasketService } from '../../core/services/basket.service';
import { AuthService } from '../../core/services/auth.service';
import { ProductSummary, ProductQuery } from '../../shared/models/product.model';
import { PaginationConstants } from '../../shared/constants/pagination.constants';
import { CatalogConstants, SortOptions, SortValue } from '../../shared/constants/catalog.constants';

@Component({
  selector: 'app-catalog',
  imports: [RouterLink, FormsModule],
  templateUrl: './catalog.html',
  styleUrl: './catalog.scss'
})
export class Catalog implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  protected readonly catalogSvc = inject(CatalogService);
  private readonly basketSvc = inject(BasketService);
  protected readonly auth = inject(AuthService);

  protected readonly products = signal<ProductSummary[]>([]);
  protected readonly totalItems = signal(0);
  protected readonly totalPages = signal(0);
  protected readonly loading = signal(true);
  protected readonly addingToBasket = signal<string | null>(null);
  protected readonly failedImages = signal<Set<string>>(new Set());
  protected readonly openPanel = signal<'filters' | null>(null);
  protected readonly collapsedSections = signal<Set<string>>(new Set());
  protected readonly showAllCategories = signal(false);
  protected readonly showAllBrands = signal(false);
  protected readonly categorySlugSig = signal('');

  protected search = '';
  protected sort: SortValue = CatalogConstants.defaultSort;
  protected availableOnly = false;
  protected offersOnly = false;
  protected minPrice = '';
  protected maxPrice = '';
  protected brandSlug = '';
  protected currentPage = 1;
  protected readonly pageSize = PaginationConstants.defaultPageSize;
  protected categorySlug = '';

  readonly sortOptions = SortOptions;

  protected readonly categoryName = computed(() => {
    const cats = this.catalogSvc.categories();
    const slug = this.categorySlugSig();
    const top = cats.find(c => c.slug === slug);
    if (top) return top.name;
    for (const cat of cats) {
      const sub = cat.subCategories?.find(s => s.slug === slug);
      if (sub) return sub.name;
    }
    return 'shop all selqi pay';
  });

  protected readonly activeTopCategory = computed(() => {
    const cats = this.catalogSvc.categories();
    const slug = this.categorySlugSig();
    if (!slug) return null;
    const direct = cats.find(c => c.slug === slug);
    if (direct) return direct;
    return cats.find(c => c.subCategories?.some(s => s.slug === slug)) ?? null;
  });

  protected readonly selectedBrandName = computed(() => {
    const brand = this.catalogSvc.brands().find(item => item.slug === this.brandSlug);
    return brand?.name ?? 'Brands';
  });

  protected readonly hasActiveFilters = computed(() => {
    return !!this.search || !!this.categorySlugSig() || !!this.brandSlug || this.availableOnly ||
      this.offersOnly || !!this.minPrice || !!this.maxPrice || this.sort !== CatalogConstants.defaultSort;
  });

  protected readonly pages = computed(() =>
    Array.from({ length: this.totalPages() }, (_, i) => i + 1)
  );

  protected readonly visibleCategories = computed(() => {
    const cats = this.catalogSvc.categories();
    return this.showAllCategories() ? cats : cats.slice(0, CatalogConstants.visibleCategoriesCount);
  });

  protected readonly visibleBrands = computed(() => {
    const brands = this.catalogSvc.brands();
    return this.showAllBrands() ? brands : brands.slice(0, CatalogConstants.visibleBrandsCount);
  });

  ngOnInit(): void {
    this.catalogSvc.loadCategories();
    this.catalogSvc.loadBrands();
    combineLatest([this.route.paramMap, this.route.queryParamMap]).subscribe(([params, query]) => {
      this.categorySlug = params.get('slug') ?? '';
      this.categorySlugSig.set(this.categorySlug);
      this.readQueryParams(query);
      this.currentPage = 1;
      this.loadProducts();
    });
  }

  private readQueryParams(query = this.route.snapshot.queryParamMap): void {
    this.search = query.get('search') ?? '';
    this.sort = (query.get('sort') ?? CatalogConstants.defaultSort) as SortValue;
    this.availableOnly = query.get('availableOnly') === 'true';
    this.offersOnly = query.get('offersOnly') === 'true';
    this.minPrice = query.get('minPrice') ?? '';
    this.maxPrice = query.get('maxPrice') ?? '';
    this.brandSlug = query.get('brand') ?? '';
  }

  protected loadProducts(): void {
    this.loading.set(true);
    const query: ProductQuery = {
      categorySlug: this.categorySlug || undefined,
      search: this.search || undefined,
      brandSlug: this.brandSlug || undefined,
      sort: (this.sort !== 'featured' ? this.sort : undefined) as ProductQuery['sort'],
      availableOnly: this.availableOnly || undefined,
      offersOnly: this.offersOnly || undefined,
      minPrice: this.minPrice ? +this.minPrice : undefined,
      maxPrice: this.maxPrice ? +this.maxPrice : undefined,
      page: this.currentPage,
      pageSize: this.pageSize
    };
    this.catalogSvc.getProducts(query).subscribe({
      next: result => {
        this.products.set(result.items);
        this.totalItems.set(result.totalItems);
        this.totalPages.set(result.totalPages);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  protected applyFilters(): void {
    this.currentPage = 1;
    this.openPanel.set(null);
    this.syncQueryParams();
  }

  protected clearFilters(): void {
    this.search = '';
    this.sort = CatalogConstants.defaultSort;
    this.availableOnly = false;
    this.offersOnly = false;
    this.minPrice = '';
    this.maxPrice = '';
    this.brandSlug = '';
    this.currentPage = 1;
    this.openPanel.set(null);
    if (this.categorySlug) {
      this.router.navigate(['/products'], { queryParams: {} });
    } else {
      this.syncQueryParams();
    }
  }

  private static readonly ALL_SECTIONS = CatalogConstants.filterSections;

  protected openDrawerOnSection(section: string): void {
    this.collapsedSections.set(new Set(Catalog.ALL_SECTIONS.filter(s => s !== section)));
    this.openPanel.set('filters');
  }

  protected openDrawerAll(): void {
    this.collapsedSections.set(new Set());
    this.openPanel.set('filters');
  }

  protected closeDrawer(): void {
    this.openPanel.set(null);
  }

  protected toggleSection(section: string): void {
    this.collapsedSections.update(s => {
      const next = new Set(s);
      next.has(section) ? next.delete(section) : next.add(section);
      return next;
    });
  }

  protected isSectionCollapsed(section: string): boolean {
    return this.collapsedSections().has(section);
  }

  protected toggleBrandChip(slug: string): void {
    this.brandSlug = this.brandSlug === slug ? '' : slug;
  }

  protected toggleShowMoreCategories(): void {
    this.showAllCategories.update(v => !v);
  }

  protected toggleShowMoreBrands(): void {
    this.showAllBrands.update(v => !v);
  }

  @HostListener('document:keydown.escape')
  protected onEscape(): void {
    if (this.openPanel() === 'filters') this.closeDrawer();
  }

  protected setSort(value: string): void {
    this.sort = value as SortValue;
    this.applyFilters();
  }

  protected showNew(): void {
    this.sort = 'new';
    this.applyFilters();
  }

  protected toggleOffers(): void {
    this.offersOnly = !this.offersOnly;
    this.applyFilters();
  }

  protected selectBrand(slug: string): void {
    this.brandSlug = slug;
    this.applyFilters();
  }

  protected selectCategory(slug: string): void {
    this.openPanel.set(null);
    this.router.navigate(slug ? ['/category', slug] : ['/products'], {
      queryParams: this.currentQueryParams()
    });
  }

  private syncQueryParams(): void {
    this.router.navigate([], { relativeTo: this.route, queryParams: this.currentQueryParams() });
  }

  private currentQueryParams(): Record<string, string | boolean | null> {
    return {
      search: this.search || null,
      brand: this.brandSlug || null,
      sort: this.sort !== CatalogConstants.defaultSort ? this.sort : null,
      availableOnly: this.availableOnly || null,
      offersOnly: this.offersOnly || null,
      minPrice: this.minPrice || null,
      maxPrice: this.maxPrice || null
    };
  }

  protected goToPage(page: number): void {
    if (page < 1 || page > this.totalPages()) return;
    this.currentPage = page;
    this.loadProducts();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  protected addToBasket(product: ProductSummary, event: Event): void {
    event.preventDefault();
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }
    this.addingToBasket.set(product.id);
    this.basketSvc.addItem({ productId: product.id, quantity: 1 }).subscribe({
      next: () => this.addingToBasket.set(null),
      error: () => this.addingToBasket.set(null)
    });
  }

  protected markImageFailed(productId: string): void {
    this.failedImages.update(items => new Set(items).add(productId));
  }

  protected imageAvailable(product: ProductSummary): boolean {
    return !!product.primaryImageUrl && !this.failedImages().has(product.id);
  }

  protected discount(product: ProductSummary): number | null {
    if (!product.wasPrice || product.wasPrice <= product.price) return null;
    return Math.round((1 - product.price / product.wasPrice) * 100);
  }

  protected badgeText(product: ProductSummary): string {
    if (this.discount(product)) return 'special offer';
    if (product.rating >= CatalogConstants.badgeRatingThreshold) return 'new';
    return product.isAvailable ? '2 year warranty' : 'limited stock';
  }

  protected badgeTone(product: ProductSummary): 'red' | 'blue' | 'green' | 'gray' {
    if (this.discount(product)) return 'red';
    if (product.rating >= CatalogConstants.badgeRatingThreshold) return 'blue';
    return product.isAvailable ? 'green' : 'gray';
  }
}
