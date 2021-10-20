import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IOrder } from 'src/app/shared/models/order';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-orders-detail',
  templateUrl: './orders-detail.component.html',
  styleUrls: ['./orders-detail.component.scss']
})
export class OrdersDetailComponent implements OnInit {
  order: IOrder;

  constructor(private ordersService: OrdersService, private activatedRoute: ActivatedRoute, private bcService: BreadcrumbService) { }

  ngOnInit(): void {
    this.loadOrder();
  }

  loadOrder(){
    this.ordersService.getOrder(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(response => {
      this.order = response;
      this.bcService.set('@orderDetails', 'Order# ' + this.order.id + ' - ' + this.order.status);
    }, error => {
      console.log(error);
    });
  }
}
