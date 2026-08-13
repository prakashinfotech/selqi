export interface BasketItem {
  id: string;
  productId: string;
  productName: string;
  productSlug: string;
  productImageUrl: string | null;
  unitPrice: number;
  wasPrice: number | null;
  quantity: number;
  lineTotal: number;
}

export interface Basket {
  id: string;
  items: BasketItem[];
  itemCount: number;
  subTotal: number;
}

export interface AddToBasketRequest {
  productId: string;
  quantity: number;
}

export interface UpdateBasketItemRequest {
  quantity: number;
}
