import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ToastController } from '@ionic/angular';
import { Router } from '@angular/router';
import { SecurityService } from 'src/app/api/security.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {

  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')]),
    password: new FormControl('', Validators.required)
  });

  constructor(private toastController: ToastController, private router: Router, private securityService: SecurityService) { }

  ngOnInit() {
  }

  ionViewDidEnter() {
    // Clear form values
    this.loginForm.reset();
  }

  onLogin() {

    this.securityService.login(this.loginForm.value.email, this.loginForm.value.password).subscribe(response => {

      const userLogged = this.securityService.getCurrentLoggedUser();

      if (userLogged) {

        if (userLogged.role.toLowerCase() === 'miembro') {

          if (this.securityService.checkExpiration()) {
            this.router.navigate(['/schedule']);
          }

        } else if (userLogged.role.toLowerCase() === 'admin') {
          this.router.navigate(['/frontdesk']);
        }
      }

    });

  }

  async showToast() {
    const toast = await this.toastController.create({
      message: '<ion-icon name="close-circle-outline"></ion-icon> Credenciales no validas',
      duration: 2000,
      color: 'danger',
      position: 'middle'
    });
    toast.present();
  }
}
