import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminService } from '../../../core/services/admin.service';
import { AuditLog } from '../../../shared/models/admin.model';

@Component({
  selector: 'app-admin-audit',
  imports: [CommonModule, FormsModule, DatePipe],
  templateUrl: './admin-audit.html',
  styleUrl: './admin-audit.scss'
})
export class AdminAuditPage implements OnInit {
  private readonly adminService = inject(AdminService);

  protected readonly logs = signal<AuditLog[]>([]);
  protected readonly search = signal('');
  protected readonly isLoading = signal(true);
  protected readonly error = signal<string | null>(null);

  protected readonly filtered = computed(() => {
    const q = this.search().toLowerCase();
    return q
      ? this.logs().filter(l => l.action.toLowerCase().includes(q) || l.summary.toLowerCase().includes(q) || l.actorEmail.toLowerCase().includes(q) || l.entityType.toLowerCase().includes(q))
      : this.logs();
  });

  ngOnInit(): void {
    this.adminService.getAuditLogs().subscribe({
      next: list => { this.logs.set(list); this.isLoading.set(false); },
      error: () => { this.error.set('Audit logs could not be loaded.'); this.isLoading.set(false); }
    });
  }

  protected actionPill(action: string): string {
    if (action.includes('Created')) return 'pill--green';
    if (action.includes('Updated') || action.includes('Inventory')) return 'pill--blue';
    if (action.includes('Deleted')) return 'pill--red';
    if (action.includes('Status')) return 'pill--purple';
    return 'pill--gray';
  }
}
