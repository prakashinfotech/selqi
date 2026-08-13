export interface DeliveryAddress {
  firstName: string;
  lastName: string;
  line1: string;
  line2?: string;
  town: string;
  county?: string;
  postcode: string;
  phone: string;
}

export interface Payment {
  cardNumber?: string;
  cardHolder?: string;
  expiryMonth?: string;
  expiryYear?: string;
  savedCardId?: string;
  saveCard?: boolean;
  cardBrand?: string;
}

export interface CheckoutRequest {
  deliveryAddress: DeliveryAddress;
  deliveryOption: 'standard' | 'express' | 'nominated';
  payment: Payment;
}

export interface OrderItem {
  productId: string;
  productName: string;
  productSku: string;
  productImageUrl: string | null;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}

export interface OrderSummary {
  id: string;
  orderNumber: string;
  status: string;
  itemCount: number;
  total: number;
  placedAt: string;
}

export interface OrderStatusHistoryEntry {
  status: string;
  changedAt: string;
  note: string | null;
}

export interface OrderDetail {
  id: string;
  orderNumber: string;
  status: string;
  items: OrderItem[];
  subTotal: number;
  deliveryCharge: number;
  total: number;
  deliveryName: string;
  deliveryAddress: string;
  paymentReference: string;
  placedAt: string;
  statusHistory: OrderStatusHistoryEntry[];
}

export interface WishlistItem {
  id: string;
  productId: string;
  productName: string;
  productSlug: string;
  productImageUrl: string | null;
  price: number;
  wasPrice: number | null;
  isAvailable: boolean;
  addedAt: string;
}

export interface Wishlist {
  items: WishlistItem[];
}
