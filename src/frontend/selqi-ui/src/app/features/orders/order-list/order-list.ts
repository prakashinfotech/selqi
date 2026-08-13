import { Component, inject, signal, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { OrderService } from '../../../core/services/order.service';
import { OrderSummary } from '../../../shared/models/order.model';

@Component({
  selector: 'app-order-list',
  imports: [RouterLink, DatePipe],
  templateUrl: './order-list.html',
  styleUrl: './order-list.scss'
})
export class OrderList implements OnInit {
  private readonly orderSvc = inject(OrderService);

  protected readonly orders = signal<OrderSummary[]>([]);
  protected readonly loading = signal(true);

  ngOnInit(): void {
    this.orderSvc.getOrders().subscribe({
      next: orders => { this.orders.set(orders); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }
}
