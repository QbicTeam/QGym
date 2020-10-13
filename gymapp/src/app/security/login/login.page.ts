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
    email: new FormControl('', [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]),
    password: new FormControl('', Validators.required)
  });

  constructor(private toastController: ToastController, private router: Router, private securityService: SecurityService) { }

  ngOnInit() {
  }

  onLogin() {
    console.log('Form:', this.loginForm.value);
    console.log(this.loginForm.value.email);

    this.securityService.login(this.loginForm.value.email, this.loginForm.value.password).subscribe(response => {
      console.log(response);
      // TODO: Determinar el tipo de usuario, meter el token en el storage, que pasa si la vigencia esta caducada?
    });

    // // this.showToast();
    // if (this.loginForm.value.email === 'gerardo@domain.com') {
    //   this.router.navigate(['/frontdesk']);
    // } else {
    //   this.router.navigate(['/schedule']);
    // }
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
