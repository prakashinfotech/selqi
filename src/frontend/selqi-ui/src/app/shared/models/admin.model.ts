export interface AdminDashboard {
  productCount: number;
  outOfStockProductCount: number;
  orderCount: number;
  openOrderCount: number;
  totalRevenue: number;
  recentOrders: AdminOrder[];
}

export interface AdminInventory {
  storeId: string;
  storeName: string;
  town: string;
  quantityAvailable: number;
  lowStockThreshold: number;
  isLowStock: boolean;
  lastRestockedAtUtc: string | null;
}

export interface AdminStore {
  id: string;
  name: string;
  addressLine1: string;
  addressLine2: string | null;
  town: string;
  postcode: string;
  phone: string | null;
  storeType: string;
  latitude: number | null;
  longitude: number | null;
  isActive: boolean;
  totalStockItems: number;
  lowStockProductCount: number;
  createdAtUtc: string;
  updatedAtUtc: string;
}

export interface AdminStoreInventoryItem {
  productId: string;
  productName: string;
  productSku: string;
  quantityAvailable: number;
  lowStockThreshold: number;
  isLowStock: boolean;
  lastRestockedAtUtc: string | null;
}

export interface AdminStoreDetail extends AdminStore {
  inventory: AdminStoreInventoryItem[];
}

export interface CreateStoreRequest {
  name: string;
  addressLine1: string;
  addressLine2: string | null;
  town: string;
  postcode: string;
  phone: string | null;
  storeType: string;
  latitude: number | null;
  longitude: number | null;
}

export interface UpdateStoreRequest extends CreateStoreRequest {
  isActive: boolean;
}

export interface UpdateStoreInventoryRequest {
  productId: string;
  quantityAvailable: number;
  lowStockThreshold: number;
}

export interface AdminProduct {
  id: string;
  name: string;
  slug: string;
  sku: string;
  categoryName: string;
  brandName: string;
  shortDescription: string;
  fullDescription: string | null;
  price: number;
  wasPrice: number | null;
  isFeatured: boolean;
  isDeleted: boolean;
  totalStock: number;
  updatedAtUtc: string | null;
  inventory: AdminInventory[];
}

export interface CreateProductRequest {
  categoryId: string;
  brandId: string;
  name: string;
  sku: string;
  shortDescription: string;
  fullDescription: string | null;
  price: number;
  wasPrice: number | null;
  isFeatured: boolean;
}

export interface UpdateAdminProductRequest {
  name: string;
  shortDescription: string;
  fullDescription: string | null;
  price: number;
  wasPrice: number | null;
  isFeatured: boolean;
  isDeleted: boolean;
  inventory: Array<{
    storeId: string;
    quantityAvailable: number;
  }>;
}

export interface AdminOrder {
  id: string;
  orderNumber: string;
  status: string;
  customerName: string;
  customerEmail: string;
  itemCount: number;
  total: number;
  placedAt: string;
  updatedAt: string;
}

export interface AuditLog {
  id: string;
  actorUserId: string;
  actorEmail: string;
  action: string;
  entityType: string;
  entityId: string;
  summary: string;
  createdAtUtc: string;
}
