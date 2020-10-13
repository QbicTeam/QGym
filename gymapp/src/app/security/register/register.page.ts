import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.page.html',
  styleUrls: ['./register.page.scss'],
})
export class RegisterPage implements OnInit {

  memberConfirmed = false;
  confirmationCodeSent = false;
  confirmationCode = '';

  registerForm = new FormGroup({
    email: new FormControl('', [Validators.required,
      Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]),
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


  constructor(private formBuilder: FormBuilder, private router: Router) { }

  ngOnInit() {
  }

  onSendConfirmationCode() {
    this.confirmationCodeSent = true;
    this.confirmationCodeForm.patchValue({confirmationCodeSent : 12345 });
    console.log(this.confirmationCodeForm.value);
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
    this.router.navigate(['/schedule']);
  }
}
