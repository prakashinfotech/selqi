export const SortOptions = [
  { value: 'featured',   label: 'Relevance' },
  { value: 'price-asc',  label: 'Price: Low to high' },
  { value: 'price-desc', label: 'Price: High to low' },
  { value: 'rating',     label: 'Top Rated' },
  { value: 'name',       label: 'Name A-Z' },
  { value: 'new',        label: 'Newest' },
] as const;

export type SortValue = typeof SortOptions[number]['value'];

export const CatalogConstants = {
  defaultSort: 'featured' as SortValue,
  visibleCategoriesCount: 4,
  visibleBrandsCount: 8,
  badgeRatingThreshold: 4.6,
  filterSections: ['search', 'category', 'brands', 'price', 'options'] as const,
  carouselIntervalMs: 5000,
  scrollThresholdPx: 4,
} as const;
