export interface Category {
  id: string;
  name: string;
  slug: string;
  description: string | null;
  imageUrl: string | null;
  displayOrder: number;
  parentCategoryId: string | null;
  subCategories: Category[];
}

export interface Brand {
  id: string;
  name: string;
  slug: string;
}

export interface ProductImage {
  url: string;
  altText: string;
  displayOrder: number;
}

export interface ProductSpecification {
  name: string;
  value: string;
  displayOrder: number;
}

export interface ProductAvailability {
  storeId: string;
  storeName: string;
  town: string;
  postcode: string;
  quantityAvailable: number;
}

export interface ProductSummary {
  id: string;
  name: string;
  slug: string;
  categoryName: string;
  categorySlug: string;
  brandName: string;
  price: number;
  wasPrice: number | null;
  rating: number;
  reviewCount: number;
  isAvailable: boolean;
  primaryImageUrl: string | null;
}

export interface ProductDetail {
  id: string;
  name: string;
  slug: string;
  categoryName: string;
  categorySlug: string;
  brandName: string;
  shortDescription: string;
  fullDescription: string | null;
  sku: string;
  price: number;
  wasPrice: number | null;
  rating: number;
  reviewCount: number;
  isAvailable: boolean;
  images: ProductImage[];
  specifications: ProductSpecification[];
  availability: ProductAvailability[];
}

export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
}

export interface ProductQuery {
  search?: string;
  categorySlug?: string;
  brandSlug?: string;
  minPrice?: number;
  maxPrice?: number;
  availableOnly?: boolean;
  offersOnly?: boolean;
  sort?: 'name' | 'price-asc' | 'price-desc' | 'rating' | 'new';
  page?: number;
  pageSize?: number;
}
