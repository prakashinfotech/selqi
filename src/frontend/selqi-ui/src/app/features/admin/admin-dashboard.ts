import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { AdminService } from '../../core/services/admin.service';
import { AdminDashboard, AdminOrder, AdminStore } from '../../shared/models/admin.model';

@Component({
  selector: 'app-admin-dashboard',
  imports: [CommonModule, RouterLink, CurrencyPipe, DatePipe],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.scss'
})
export class AdminDashboardPage implements OnInit {
  private readonly adminService = inject(AdminService);

  protected readonly dashboard = signal<AdminDashboard | null>(null);
  protected readonly recentOrders = signal<AdminOrder[]>([]);
  protected readonly stores = signal<AdminStore[]>([]);
  protected readonly isLoading = signal(true);
  protected readonly error = signal<string | null>(null);

  ngOnInit(): void {
    forkJoin({
      dashboard: this.adminService.getDashboard(),
      stores: this.adminService.getStores()
    }).subscribe({
      next: result => {
        this.dashboard.set(result.dashboard);
        this.recentOrders.set(result.dashboard.recentOrders);
        this.stores.set(result.stores);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Could not load overview data.');
        this.isLoading.set(false);
      }
    });
  }

  protected statusPill(status: string): string {
    const map: Record<string, string> = {
      Pending: 'pill--amber', Confirmed: 'pill--blue', Processing: 'pill--blue',
      Shipped: 'pill--purple', Delivered: 'pill--green', Cancelled: 'pill--gray'
    };
    return map[status] ?? 'pill--gray';
  }

  protected typePill(type: string): string {
    return { Standalone: 'pill--blue', Concession: 'pill--amber', CollectionPoint: 'pill--purple' }[type] ?? 'pill--gray';
  }
}
