import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.page.html',
  styleUrls: ['./payment.page.scss'],
})
export class PaymentPage implements OnInit {

  currentPkg: any;

  // TODO: debe desplegar un mensaje que la suscripcion esta vencida.
  // Obtener el detalle del paquete que se selecciono.

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit() {

    this.activatedRoute.paramMap.subscribe(paramMap => {

      this.currentPkg = {
        id: +paramMap.get('pkgid'),
        name: paramMap.get('name'),
        price: +paramMap.get('price')
        };
    });

  }
}
