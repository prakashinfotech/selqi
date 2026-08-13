export interface SavedCard {
  id: string;
  cardholderName: string;
  last4: string;
  brand: string;
  expiryMonth: string;
  expiryYear: string;
  isDefault: boolean;
  createdAt: string;
}

export interface SaveCardRequest {
  cardholderName: string;
  last4: string;
  brand: string;
  expiryMonth: string;
  expiryYear: string;
  setAsDefault: boolean;
}
