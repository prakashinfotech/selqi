import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./features/home/home').then(m => m.Home)
  },
  {
    path: 'products',
    loadComponent: () => import('./features/catalog/catalog').then(m => m.Catalog)
  },
  {
    path: 'category/:slug',
    loadComponent: () => import('./features/catalog/catalog').then(m => m.Catalog)
  },
  {
    path: 'product/:slug',
    loadComponent: () => import('./features/product/product').then(m => m.Product)
  },
  {
    path: 'basket',
    loadComponent: () => import('./features/basket/basket').then(m => m.Basket)
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then(m => m.Login)
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register').then(m => m.Register)
  },
  {
    path: 'forgot-password',
    loadComponent: () => import('./features/auth/forgot-password/forgot-password').then(m => m.ForgotPassword)
  },
  {
    path: 'reset-password',
    loadComponent: () => import('./features/auth/reset-password/reset-password').then(m => m.ResetPassword)
  },
  {
    path: 'checkout',
    canActivate: [authGuard],
    loadComponent: () => import('./features/checkout/checkout').then(m => m.Checkout)
  },
  {
    path: 'orders',
    canActivate: [authGuard],
    loadComponent: () => import('./features/orders/order-list/order-list').then(m => m.OrderList)
  },
  {
    path: 'orders/:orderNumber',
    canActivate: [authGuard],
    loadComponent: () => import('./features/orders/confirmation/confirmation').then(m => m.Confirmation)
  },
  {
    path: 'wishlist',
    canActivate: [authGuard],
    loadComponent: () => import('./features/wishlist/wishlist').then(m => m.WishlistPage)
  },
  {
    path: 'admin',
    canActivate: [authGuard, adminGuard],
    loadComponent: () => import('./features/admin/admin-shell').then(m => m.AdminShell),
    children: [
      { path: '', loadComponent: () => import('./features/admin/admin-dashboard').then(m => m.AdminDashboardPage) },
      { path: 'products', loadComponent: () => import('./features/admin/products/admin-products').then(m => m.AdminProductsPage) },
      { path: 'stores', loadComponent: () => import('./features/admin/stores/admin-stores').then(m => m.AdminStoresPage) },
      { path: 'orders', loadComponent: () => import('./features/admin/orders/admin-orders').then(m => m.AdminOrdersPage) },
      { path: 'audit', loadComponent: () => import('./features/admin/audit/admin-audit').then(m => m.AdminAuditPage) },
    ]
  },
  {
    path: '**',
    redirectTo: ''
  }
];
