import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersComponent } from './orders.component';
import { OrdersDetailComponent } from './orders-detail/orders-detail.component';
import { OrdersRoutingModule } from './orders-routing.module';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [
    OrdersComponent,
    OrdersDetailComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    OrdersRoutingModule
  ]
})
export class OrdersModule { }
