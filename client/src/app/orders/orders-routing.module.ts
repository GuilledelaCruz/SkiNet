import { NgModule } from '@angular/core';
import { OrdersComponent } from './orders.component';
import { OrdersDetailComponent } from './orders-detail/orders-detail.component';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {path: '', component: OrdersComponent},
  {path: ':id', component: OrdersDetailComponent, data: {breadcrumb: {alias: 'orderDetails'}}}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class OrdersRoutingModule { }
