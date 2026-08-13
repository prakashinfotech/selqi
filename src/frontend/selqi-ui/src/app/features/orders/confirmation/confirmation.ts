import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { OrderService } from '../../../core/services/order.service';
import { OrderDetail } from '../../../shared/models/order.model';

const STATUS_STEPS = ['Pending', 'Confirmed', 'Processing', 'Shipped', 'Delivered'] as const;
type StatusStep = typeof STATUS_STEPS[number];

const STATUS_LABELS: Record<string, string> = {
  Pending: 'Order Placed',
  Confirmed: 'Confirmed',
  Processing: 'Processing',
  Shipped: 'Shipped',
  Delivered: 'Delivered',
  Cancelled: 'Cancelled'
};

const BANNER_MESSAGES: Record<string, { heading: string; sub: string }> = {
  Pending:    { heading: 'Order placed!',     sub: "We've received your order and will confirm it shortly." },
  Confirmed:  { heading: 'Order confirmed!',  sub: "Your order has been confirmed and is being prepared." },
  Processing: { heading: 'Being prepared!',   sub: "Your items are being picked and packed." },
  Shipped:    { heading: "It's on its way!",  sub: "Your order has been shipped and is en route to you." },
  Delivered:  { heading: 'Delivered!',        sub: "Your order has been delivered. Enjoy!" },
  Cancelled:  { heading: 'Order cancelled',   sub: "This order has been cancelled." }
};

@Component({
  selector: 'app-confirmation',
  imports: [RouterLink, DatePipe],
  templateUrl: './confirmation.html',
  styleUrl: './confirmation.scss'
})
export class Confirmation implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly orderSvc = inject(OrderService);

  protected readonly order = signal<OrderDetail | null>(null);
  protected readonly loading = signal(true);

  protected readonly isCancelled = computed(() => this.order()?.status === 'Cancelled');

  protected readonly banner = computed(() => {
    const status = this.order()?.status ?? 'Pending';
    return BANNER_MESSAGES[status] ?? BANNER_MESSAGES['Pending'];
  });

  protected readonly activeStepIndex = computed(() => {
    const status = this.order()?.status ?? 'Pending';
    if (status === 'Cancelled') return -1;
    return STATUS_STEPS.indexOf(status as StatusStep);
  });

  protected readonly statusSteps = STATUS_STEPS;

  protected stepState(stepName: string): 'done' | 'active' | 'upcoming' {
    if (this.isCancelled()) return 'upcoming';
    const stepIdx = STATUS_STEPS.indexOf(stepName as StatusStep);
    const active = this.activeStepIndex();
    if (stepIdx < active) return 'done';
    if (stepIdx === active) return 'active';
    return 'upcoming';
  }

  protected stepLabel(step: string): string {
    return STATUS_LABELS[step] ?? step;
  }

  ngOnInit(): void {
    const orderNumber = this.route.snapshot.paramMap.get('orderNumber')!;
    this.orderSvc.getOrder(orderNumber).subscribe({
      next: o => { this.order.set(o); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }
}
