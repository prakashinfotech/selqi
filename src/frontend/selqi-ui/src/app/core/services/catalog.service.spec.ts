import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { firstValueFrom } from 'rxjs';
import { CatalogService } from './catalog.service';
import { environment } from '../../../environments/environment';
import { PagedResult, ProductSummary, Category, Brand } from '../../shared/models/product.model';

const API = environment.apiBase;

describe('CatalogService', () => {
  let service: CatalogService;
  let http: HttpTestingController;

  const mockCategory: Category = {
    id: 'cat-1', name: 'Electronics', slug: 'electronics',
    description: null, imageUrl: null, displayOrder: 1,
    parentCategoryId: null, subCategories: []
  };
  const mockBrand: Brand = { id: 'brand-1', name: 'Selqi', slug: 'selqi' };
  const mockProduct: ProductSummary = {
    id: 'prod-1', name: 'Smart TV', slug: 'smart-tv',
    categoryName: 'Electronics', categorySlug: 'electronics',
    brandName: 'Selqi', price: 299.99, wasPrice: null,
    rating: 4.5, reviewCount: 10, primaryImageUrl: null, isAvailable: true
  };
  const pagedResult: PagedResult<ProductSummary> = {
    items: [mockProduct], page: 1, pageSize: 24, totalItems: 1, totalPages: 1
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        CatalogService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(CatalogService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  // ── Positive ────────────────────────────────────────────────────────────

  it('getProducts sends GET to /products with default pageSize', () => {
    service.getProducts().subscribe();

    const req = http.expectOne(r => r.url === `${API}/products`);
    expect(req.request.method).toBe('GET');
    expect(req.request.params.get('pageSize')).toBe('24');
    req.flush(pagedResult);
  });

  it('getProducts passes search and category filters as query params', () => {
    service.getProducts({ search: 'tv', categorySlug: 'electronics' }).subscribe();

    const req = http.expectOne(r => r.url === `${API}/products`);
    expect(req.request.params.get('search')).toBe('tv');
    expect(req.request.params.get('category')).toBe('electronics');
    req.flush(pagedResult);
  });

  it('getProductBySlug sends GET to /products/:slug', () => {
    service.getProductBySlug('smart-tv').subscribe(result => {
      expect(result.name).toBe('Smart TV');
    });

    http.expectOne(`${API}/products/smart-tv`).flush(mockProduct);
  });

  it('loadCategories populates categories signal on success', () => {
    service.loadCategories();

    http.expectOne(`${API}/categories`).flush([mockCategory]);

    expect(service.categories()).toHaveLength(1);
    expect(service.categories()[0].slug).toBe('electronics');
    expect(service.categoriesLoaded()).toBe(true);
  });

  it('loadCategories does not make a second request if already loaded', () => {
    service.loadCategories();
    http.expectOne(`${API}/categories`).flush([mockCategory]);

    service.loadCategories();
    http.expectNone(`${API}/categories`);
  });

  it('getBrands sends GET to /brands', () => {
    service.getBrands().subscribe(brands => {
      expect(brands).toHaveLength(1);
    });

    http.expectOne(`${API}/brands`).flush([mockBrand]);
  });

  // ── Negative ─────────────────────────────────────────────────────────────

  it('getProducts propagates HTTP 500 error to subscriber', async () => {
    const promise = firstValueFrom(service.getProducts()).catch(e => e);

    http.expectOne(r => r.url === `${API}/products`).flush(
      { message: 'Server error' },
      { status: 500, statusText: 'Internal Server Error' }
    );

    const err = await promise;
    expect(err.status).toBe(500);
  });

  it('getProductBySlug propagates HTTP 404 when product not found', async () => {
    const promise = firstValueFrom(service.getProductBySlug('no-such-product')).catch(e => e);

    http.expectOne(`${API}/products/no-such-product`).flush(
      { message: 'Not found' },
      { status: 404, statusText: 'Not Found' }
    );

    const err = await promise;
    expect(err.status).toBe(404);
  });

  it('loadCategories sets categoriesLoaded true even when request fails', () => {
    service.loadCategories();

    http.expectOne(`${API}/categories`).flush(
      { message: 'Server error' },
      { status: 500, statusText: 'Internal Server Error' }
    );

    expect(service.categoriesLoaded()).toBe(true);
    expect(service.categories()).toHaveLength(0);
  });
});
