import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GymService } from 'src/app/api/gym.service';
import { SecurityService } from 'src/app/api/security.service';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.page.html',
  styleUrls: ['./payment.page.scss'],
})
export class PaymentPage implements OnInit {

  currentPkg: any;
  currentUser: any;

  // TODO: debe desplegar un mensaje que la suscripcion esta vencida.
  // Obtener el detalle del paquete que se selecciono.

  constructor(private activatedRoute: ActivatedRoute, private gymService: GymService,
              private router: Router, private securityService: SecurityService) { }

  ngOnInit() {

    this.activatedRoute.paramMap.subscribe(paramMap => {

      this.currentPkg = {
        id: +paramMap.get('pkgid'),
        name: paramMap.get('name'),
        price: +paramMap.get('price')
        };
    });

    this.currentUser = this.securityService.getCurrentLoggedUser();

  }

  payMembership() {

    this.gymService.payMembership(this.currentUser.id, this.currentPkg.id, this.currentPkg.price).subscribe(() => {
      this.router.navigate(['/schedule']);
    });

  }
}
