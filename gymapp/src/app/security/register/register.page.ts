import { ThrowStmt } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastController } from '@ionic/angular';
import { SecurityService } from 'src/app/api/security.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.page.html',
  styleUrls: ['./register.page.scss'],
})
export class RegisterPage implements OnInit {

  isLoading = false;
  memberConfirmed = false;
  confirmationCodeSent = false;
  confirmationCode = '';

  registerForm = new FormGroup({
    email: new FormControl('', [Validators.required,
      Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')]),
    fullName: new FormControl('', [Validators.required, Validators.minLength(5)])
  });

  confirmationCodeForm = this.formBuilder.group({
    confirmationCode: ['', Validators.required],
    confirmationCodeSent: [''],
  }, { validator: this.confirmationCodeMatchValidator});

  passwordForm = this.formBuilder.group({
    password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
    confirmPassword: ['', Validators.required]
  }, { validator: this.passwordMatchValidator});


  constructor(private formBuilder: FormBuilder, private router: Router,
              private securityService: SecurityService) { }

  ngOnInit() {
  }

  onSendConfirmationCode() {

    this.isLoading = true;

    this.securityService.getConfirmationCodeForRegister(this.registerForm.value.email).subscribe(response => {

      this.confirmationCodeSent = true;
      this.confirmationCodeForm.patchValue({confirmationCodeSent : response });
      this.isLoading = false;

    });

  }

  onVerified() {
    this.memberConfirmed = true;
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { mismatch: true };
  }

  confirmationCodeMatchValidator(g: FormGroup) {
    return g.get('confirmationCode').value === g.get('confirmationCodeSent').value ? null : { mismatch: true };
  }

  onRegister() {

    this.isLoading = true;

    this.securityService.register(this.registerForm.value.email,
                                  this.registerForm.value.fullName,
                                  this.passwordForm.value.password).subscribe(response => {

        this.securityService.login(this.registerForm.value.email,
                                    this.passwordForm.value.password).subscribe(resLogin => {

          // TODO: Esto debe ponerse el localstorage.
          this.isLoading = false;
          this.router.navigate(['/packages']);

        });

    });
  }

}
