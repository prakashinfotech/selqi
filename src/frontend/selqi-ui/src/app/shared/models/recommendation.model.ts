import { ProductSummary } from './product.model';

export interface AiSuggestion {
  product: ProductSummary;
  reason: string;
}
