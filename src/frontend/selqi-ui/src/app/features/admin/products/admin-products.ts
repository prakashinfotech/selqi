import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { AdminService } from '../../../core/services/admin.service';
import { AdminProduct, CreateProductRequest, UpdateAdminProductRequest } from '../../../shared/models/admin.model';

type ProductModal = 'none' | 'create' | 'edit';

@Component({
  selector: 'app-admin-products',
  imports: [CommonModule, FormsModule, CurrencyPipe],
  templateUrl: './admin-products.html',
  styleUrl: './admin-products.scss'
})
export class AdminProductsPage implements OnInit {
  private readonly adminService = inject(AdminService);

  protected readonly products = signal<AdminProduct[]>([]);
  protected readonly categories = signal<{ id: string; name: string }[]>([]);
  protected readonly brands = signal<{ id: string; name: string }[]>([]);
  protected readonly search = signal('');
  protected readonly isLoading = signal(true);
  protected readonly message = signal<{ type: 'success' | 'error'; text: string } | null>(null);
  protected readonly activeModal = signal<ProductModal>('none');
  protected readonly saving = signal(false);

  protected editTarget: AdminProduct | null = null;
  protected editForm: UpdateAdminProductRequest = this.emptyEditForm();
  protected createForm: CreateProductRequest = this.emptyCreateForm();

  protected readonly filtered = computed(() => {
    const q = this.search().toLowerCase();
    return q
      ? this.products().filter(p =>
          p.name.toLowerCase().includes(q) ||
          p.sku.toLowerCase().includes(q) ||
          p.categoryName.toLowerCase().includes(q))
      : this.products();
  });

  ngOnInit(): void {
    forkJoin({
      products: this.adminService.getProducts(),
      categories: this.adminService.getCategories(),
      brands: this.adminService.getBrands()
    }).subscribe({
      next: ({ products, categories, brands }) => {
        this.products.set(products);
        this.categories.set(categories);
        this.brands.set(brands);
        this.isLoading.set(false);
      },
      error: () => { this.flash('error', 'Data could not be loaded.'); this.isLoading.set(false); }
    });
  }

  protected openCreate(): void {
    this.createForm = this.emptyCreateForm();
    if (this.categories().length) this.createForm.categoryId = this.categories()[0].id;
    if (this.brands().length) this.createForm.brandId = this.brands()[0].id;
    this.activeModal.set('create');
  }

  protected openEdit(product: AdminProduct): void {
    this.editTarget = product;
    this.editForm = {
      name: product.name,
      shortDescription: product.shortDescription,
      fullDescription: product.fullDescription,
      price: product.price,
      wasPrice: product.wasPrice,
      isFeatured: product.isFeatured,
      isDeleted: product.isDeleted,
      inventory: product.inventory.map(i => ({
        storeId: i.storeId,
        quantityAvailable: i.quantityAvailable,
        lowStockThreshold: i.lowStockThreshold
      }))
    };
    this.activeModal.set('edit');
  }

  protected closeModal(): void {
    this.activeModal.set('none');
    this.editTarget = null;
  }

  protected saveCreate(): void {
    this.saving.set(true);
    this.adminService.createProduct(this.createForm).subscribe({
      next: created => {
        this.products.update(list => [created, ...list]);
        this.flash('success', `"${created.name}" created.`);
        this.closeModal();
        this.saving.set(false);
      },
      error: res => {
        this.flash('error', res?.error?.message ?? 'Could not create product.');
        this.saving.set(false);
      }
    });
  }

  protected saveEdit(): void {
    if (!this.editTarget) return;
    this.saving.set(true);
    this.adminService.updateProduct(this.editTarget.id, this.editForm).subscribe({
      next: updated => {
        this.products.update(list => list.map(p => p.id === updated.id ? updated : p));
        this.flash('success', `"${updated.name}" saved.`);
        this.closeModal();
        this.saving.set(false);
      },
      error: res => {
        this.flash('error', res?.error?.message ?? 'Could not save product.');
        this.saving.set(false);
      }
    });
  }

  protected inventoryQty(storeId: string): number {
    return this.editForm.inventory.find(i => i.storeId === storeId)?.quantityAvailable ?? 0;
  }

  protected setQty(storeId: string, val: number): void {
    this.editForm = {
      ...this.editForm,
      inventory: this.editForm.inventory.map(i =>
        i.storeId === storeId ? { ...i, quantityAvailable: Math.max(0, val || 0) } : i)
    };
  }

  private flash(type: 'success' | 'error', text: string): void {
    this.message.set({ type, text });
    setTimeout(() => this.message.set(null), 4000);
  }

  private emptyEditForm(): UpdateAdminProductRequest {
    return { name: '', shortDescription: '', fullDescription: null, price: 0, wasPrice: null, isFeatured: false, isDeleted: false, inventory: [] };
  }

  private emptyCreateForm(): CreateProductRequest {
    return { categoryId: '', brandId: '', name: '', sku: '', shortDescription: '', fullDescription: null, price: 0, wasPrice: null, isFeatured: false };
  }
}
