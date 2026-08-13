import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminService } from '../../../core/services/admin.service';
import { AdminOrder } from '../../../shared/models/admin.model';

type OrderStatus = 'Pending' | 'Confirmed' | 'Processing' | 'Shipped' | 'Delivered' | 'Cancelled';

const ALL_STATUSES: OrderStatus[] = ['Pending', 'Confirmed', 'Processing', 'Shipped', 'Delivered', 'Cancelled'];

const VALID_TRANSITIONS: Record<OrderStatus, OrderStatus[]> = {
  Pending:    ['Confirmed', 'Cancelled'],
  Confirmed:  ['Processing', 'Cancelled'],
  Processing: ['Shipped', 'Cancelled'],
  Shipped:    ['Delivered'],
  Delivered:  [],
  Cancelled:  [],
};

@Component({
  selector: 'app-admin-orders',
  imports: [CommonModule, FormsModule, CurrencyPipe, DatePipe],
  templateUrl: './admin-orders.html',
  styleUrl: './admin-orders.scss'
})
export class AdminOrdersPage implements OnInit {
  private readonly adminService = inject(AdminService);

  protected readonly orders = signal<AdminOrder[]>([]);
  protected readonly search = signal('');
  protected readonly filterStatus = signal('');
  protected readonly isLoading = signal(true);
  protected readonly saving = signal(false);
  protected readonly modalOpen = signal(false);
  protected readonly message = signal<{ type: 'success' | 'error'; text: string } | null>(null);
  protected readonly editTarget = signal<AdminOrder | null>(null);
  protected readonly allStatuses = ALL_STATUSES;

  protected newStatus: OrderStatus | '' = '';

  protected readonly filtered = computed(() => {
    let list = this.orders();
    const q = this.search().toLowerCase();
    const st = this.filterStatus();
    if (q) list = list.filter(o =>
      o.orderNumber.toLowerCase().includes(q) ||
      o.customerName.toLowerCase().includes(q) ||
      o.customerEmail.toLowerCase().includes(q));
    if (st) list = list.filter(o => o.status === st);
    return list;
  });

  protected readonly allowedNextStatuses = computed<OrderStatus[]>(() => {
    const current = this.editTarget()?.status as OrderStatus | undefined;
    if (!current) return [];
    return VALID_TRANSITIONS[current] ?? [];
  });

  protected readonly isTerminal = computed(() => {
    const current = this.editTarget()?.status as OrderStatus | undefined;
    return current === 'Delivered' || current === 'Cancelled';
  });

  ngOnInit(): void {
    this.adminService.getOrders().subscribe({
      next: list => { this.orders.set(list); this.isLoading.set(false); },
      error: () => { this.flash('error', 'Orders could not be loaded.'); this.isLoading.set(false); }
    });
  }

  protected openStatusModal(order: AdminOrder): void {
    this.editTarget.set(order);
    const allowed = VALID_TRANSITIONS[order.status as OrderStatus] ?? [];
    this.newStatus = allowed.length > 0 ? allowed[0] : '';
    this.modalOpen.set(true);
  }

  protected closeModal(): void { this.modalOpen.set(false); this.editTarget.set(null); }

  protected saveStatus(): void {
    const target = this.editTarget();
    if (!target || !this.newStatus) { this.closeModal(); return; }
    this.saving.set(true);
    this.adminService.updateOrderStatus(target.id, this.newStatus).subscribe({
      next: updated => {
        this.orders.update(l => l.map(o => o.id === updated.id ? updated : o));
        this.flash('success', `${updated.orderNumber} → ${updated.status}.`);
        this.closeModal();
        this.saving.set(false);
      },
      error: res => { this.flash('error', res?.error?.message ?? 'Could not update status.'); this.saving.set(false); }
    });
  }

  protected isStatusBefore(s: string, current: string): boolean {
    const si = ALL_STATUSES.indexOf(s as OrderStatus);
    const ci = ALL_STATUSES.indexOf(current as OrderStatus);
    return si < ci && current !== 'Cancelled';
  }

  protected statusPill(s: string): string {
    return ({ Pending: 'pill--amber', Confirmed: 'pill--blue', Processing: 'pill--blue', Shipped: 'pill--purple', Delivered: 'pill--green', Cancelled: 'pill--gray' } as Record<string, string>)[s] ?? 'pill--gray';
  }

  private flash(type: 'success' | 'error', text: string): void {
    this.message.set({ type, text });
    setTimeout(() => this.message.set(null), 4000);
  }
}
