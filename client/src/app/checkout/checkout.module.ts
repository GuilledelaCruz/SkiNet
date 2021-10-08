import { NgModule } from '@angular/core';
import { CheckoutComponent } from './checkout.component';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {path: '', component: CheckoutComponent}
];

@NgModule({
  declarations: [
    CheckoutComponent
  ],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class CheckoutModule { }
