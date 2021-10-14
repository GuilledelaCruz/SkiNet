import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/basket';
import { CheckoutService } from '../checkout.service';
import { IOrder } from '../../shared/models/order';
import { NavigationExtras, Router } from '@angular/router';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm: FormGroup;

  constructor(private checkoutService: CheckoutService, private basketService: BasketService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void { }

  submitOrder(){
    const basket = this.basketService.getCurrentBasketValue();
    const orderToCreate = this.getOrderToCreate(basket);
    this.checkoutService.createOrder(orderToCreate).subscribe((order: IOrder) => {
      this.toastr.success('Order created successfully');
      this.basketService.deleteLocalBasket(basket.id);
      const navExtras: NavigationExtras = {state: order};
      this.router.navigate(['checkout/success'], navExtras);
    }, error =>{
      this.toastr.error('Unable to create order');
    });
  }

  getOrderToCreate(basket: IBasket) {
    return {
      basketId: basket.id,
      deliveryMethod: +this.checkoutForm.get('deliveryForm').get('deliveryMethod').value,
      shipToAddress: this.checkoutForm.get('addressForm').value
    };
  }

}
