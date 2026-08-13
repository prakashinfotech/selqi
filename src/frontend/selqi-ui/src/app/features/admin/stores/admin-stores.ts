import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminService } from '../../../core/services/admin.service';
import {
  AdminStore, AdminStoreDetail, AdminStoreInventoryItem,
  CreateStoreRequest, UpdateStoreRequest, UpdateStoreInventoryRequest
} from '../../../shared/models/admin.model';

type StoreModal = 'none' | 'create' | 'edit' | 'inventory';

interface StoreForm {
  name: string; addressLine1: string; addressLine2: string | null;
  town: string; postcode: string; phone: string | null;
  storeType: string; latitude: number | null; longitude: number | null; isActive: boolean;
}

@Component({
  selector: 'app-admin-stores',
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-stores.html',
  styleUrl: './admin-stores.scss'
})
export class AdminStoresPage implements OnInit {
  private readonly adminService = inject(AdminService);

  protected readonly stores = signal<AdminStore[]>([]);
  protected readonly storeDetail = signal<AdminStoreDetail | null>(null);
  protected readonly isLoading = signal(true);
  protected readonly detailLoading = signal(false);
  protected readonly saving = signal(false);
  protected readonly modal = signal<StoreModal>('none');
  protected readonly message = signal<{ type: 'success' | 'error'; text: string } | null>(null);
  protected readonly storeTypes = ['Standalone', 'Concession', 'CollectionPoint'];

  protected form: StoreForm = this.emptyForm();
  protected editingInv: AdminStoreInventoryItem | null = null;
  protected invQty = 0;
  protected invThreshold = 5;

  ngOnInit(): void { this.load(); }

  private load(): void {
    this.adminService.getStores().subscribe({
      next: list => { this.stores.set(list); this.isLoading.set(false); },
      error: () => { this.flash('error', 'Stores could not be loaded.'); this.isLoading.set(false); }
    });
  }

  protected openCreate(): void {
    this.form = this.emptyForm();
    this.modal.set('create');
  }

  protected openEdit(store: AdminStore): void {
    this.detailLoading.set(true);
    this.modal.set('edit');
    this.adminService.getStore(store.id).subscribe({
      next: detail => {
        this.storeDetail.set(detail);
        this.form = {
          name: detail.name, addressLine1: detail.addressLine1,
          addressLine2: detail.addressLine2, town: detail.town,
          postcode: detail.postcode, phone: detail.phone,
          storeType: detail.storeType, latitude: detail.latitude,
          longitude: detail.longitude, isActive: detail.isActive
        };
        this.detailLoading.set(false);
      },
      error: () => { this.flash('error', 'Could not load store.'); this.closeModal(); }
    });
  }

  protected openInventory(store: AdminStore): void {
    this.detailLoading.set(true);
    this.modal.set('inventory');
    this.editingInv = null;
    this.adminService.getStore(store.id).subscribe({
      next: detail => { this.storeDetail.set(detail); this.detailLoading.set(false); },
      error: () => { this.flash('error', 'Could not load store.'); this.closeModal(); }
    });
  }

  protected saveStore(): void {
    this.saving.set(true);
    if (this.modal() === 'create') {
      const req: CreateStoreRequest = { ...this.form };
      this.adminService.createStore(req).subscribe({
        next: created => {
          this.stores.update(l => [...l, created]);
          this.flash('success', `Store "${created.name}" created.`);
          this.closeModal();
          this.saving.set(false);
        },
        error: res => { this.flash('error', res?.error?.message ?? 'Could not create store.'); this.saving.set(false); }
      });
    } else {
      const detail = this.storeDetail();
      if (!detail) return;
      const req: UpdateStoreRequest = { ...this.form };
      this.adminService.updateStore(detail.id, req).subscribe({
        next: updated => {
          this.stores.update(l => l.map(s => s.id === updated.id ? updated : s));
          this.storeDetail.set(updated);
          this.flash('success', `Store "${updated.name}" saved.`);
          this.saving.set(false);
          this.closeModal();
        },
        error: res => { this.flash('error', res?.error?.message ?? 'Could not save store.'); this.saving.set(false); }
      });
    }
  }

  protected deleteStore(store: AdminStore): void {
    if (!confirm(`Delete "${store.name}"? This cannot be undone.`)) return;
    this.adminService.deleteStore(store.id).subscribe({
      next: () => {
        this.stores.update(l => l.filter(s => s.id !== store.id));
        this.flash('success', `"${store.name}" deleted.`);
      },
      error: () => this.flash('error', 'Could not delete store.')
    });
  }

  protected startEditInv(item: AdminStoreInventoryItem): void {
    this.editingInv = item;
    this.invQty = item.quantityAvailable;
    this.invThreshold = item.lowStockThreshold;
  }

  protected saveInvItem(): void {
    const detail = this.storeDetail();
    const item = this.editingInv;
    if (!detail || !item) return;
    const req: UpdateStoreInventoryRequest = { productId: item.productId, quantityAvailable: this.invQty, lowStockThreshold: this.invThreshold };
    this.adminService.updateStoreInventory(detail.id, req).subscribe({
      next: () => {
        this.storeDetail.update(s => s ? {
          ...s,
          inventory: s.inventory.map(i => i.productId === item.productId
            ? { ...i, quantityAvailable: this.invQty, lowStockThreshold: this.invThreshold, isLowStock: this.invQty > 0 && this.invQty <= this.invThreshold }
            : i)
        } : s);
        this.stores.update(l => l.map(s => s.id === detail.id
          ? { ...s, totalStockItems: s.totalStockItems - item.quantityAvailable + this.invQty }
          : s));
        this.editingInv = null;
        this.flash('success', `Stock updated for "${item.productName}".`);
      },
      error: () => this.flash('error', 'Could not update inventory.')
    });
  }

  protected closeModal(): void {
    this.modal.set('none');
    this.storeDetail.set(null);
    this.editingInv = null;
    this.saving.set(false);
  }

  protected typePill(type: string): string {
    return { Standalone: 'pill--blue', Concession: 'pill--amber', CollectionPoint: 'pill--purple' }[type] ?? 'pill--gray';
  }

  private flash(type: 'success' | 'error', text: string): void {
    this.message.set({ type, text });
    setTimeout(() => this.message.set(null), 4000);
  }

  private emptyForm(): StoreForm {
    return { name: '', addressLine1: '', addressLine2: null, town: '', postcode: '', phone: null, storeType: 'Standalone', latitude: null, longitude: null, isActive: true };
  }
}
