import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ToastController } from '@ionic/angular';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/api/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {

  loginForm = new FormGroup({
    email: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  });

  constructor(private toastController: ToastController, private router: Router, private authService: AuthService) { }

  ngOnInit() {
  }

  onLogin() {
    console.log('Form:', this.loginForm.value);
    console.log(this.loginForm.value.email);
    // this.showToast();
    if (this.loginForm.value.email === 'gerardo@domain.com') {
      this.router.navigate(['/frontdesk']);
    } else {
      this.router.navigate(['/schedule']);
    }

    this.authService.login().subscribe(response => {
      console.log(response);
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
