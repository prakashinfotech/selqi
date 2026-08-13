import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Category, Brand, ProductSummary, ProductDetail, PagedResult, ProductQuery } from '../../shared/models/product.model';
import { PaginationConstants } from '../../shared/constants/pagination.constants';
import { environment } from '../../../environments/environment';

const API_BASE = environment.apiBase;

@Injectable({ providedIn: 'root' })
export class CatalogService {
  private readonly http = inject(HttpClient);

  readonly categories = signal<Category[]>([]);
  readonly categoriesLoaded = signal(false);
  readonly brands = signal<Brand[]>([]);
  readonly brandsLoaded = signal(false);

  loadCategories(): void {
    if (this.categoriesLoaded()) return;
    this.http.get<Category[]>(`${API_BASE}/categories`).subscribe({
      next: cats => { this.categories.set(cats); this.categoriesLoaded.set(true); },
      error: () => this.categoriesLoaded.set(true)
    });
  }

  getProducts(query: ProductQuery = {}) {
    let params = new HttpParams();
    if (query.search)        params = params.set('search', query.search);
    if (query.categorySlug)  params = params.set('category', query.categorySlug);
    if (query.brandSlug)     params = params.set('brand', query.brandSlug);
    if (query.minPrice)      params = params.set('minPrice', query.minPrice);
    if (query.maxPrice)      params = params.set('maxPrice', query.maxPrice);
    if (query.availableOnly) params = params.set('availableOnly', true);
    if (query.offersOnly)    params = params.set('offersOnly', true);
    if (query.sort)          params = params.set('sort', query.sort);
    if (query.page)          params = params.set('page', query.page);
    params = params.set('pageSize', query.pageSize ?? PaginationConstants.defaultPageSize);
    return this.http.get<PagedResult<ProductSummary>>(`${API_BASE}/products`, { params });
  }

  getProductBySlug(slug: string) {
    return this.http.get<ProductDetail>(`${API_BASE}/products/${slug}`);
  }

  getBrands() {
    return this.http.get<Brand[]>(`${API_BASE}/brands`);
  }

  loadBrands(): void {
    if (this.brandsLoaded()) return;
    this.getBrands().subscribe({
      next: brands => { this.brands.set(brands); this.brandsLoaded.set(true); },
      error: () => this.brandsLoaded.set(true)
    });
  }
}
