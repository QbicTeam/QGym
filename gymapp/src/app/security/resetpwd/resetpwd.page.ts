import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SecurityService } from 'src/app/api/security.service';

@Component({
  selector: 'app-resetpwd',
  templateUrl: './resetpwd.page.html',
  styleUrls: ['./resetpwd.page.scss'],
})
export class ResetpwdPage implements OnInit {

  optionsSelection: any;
  selectedOption;
  memberEmail: string;


  identityForm = new FormGroup({
    identity: new FormControl('', Validators.required)
  });

  responseForm = new FormGroup({
    response: new FormControl('', Validators.required)
  });

  passwordForm = this.formBuilder.group({
    password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
    confirmPassword: ['', Validators.required]
  }, { validator: this.passwordMatchValidator});

  confirmationCodeForm = this.formBuilder.group({
    confirmationCode: ['', Validators.required],
    confirmationCodeSent: [''],
  }, { validator: this.confirmationCodeMatchValidator});


  constructor(private formBuilder: FormBuilder, private securityService: SecurityService,
              private router: Router) { }

  ngOnInit() {
  }

  ionViewWillEnter() {
    this.resetFormValues();
  }

  resetFormValues() {
    this.optionsSelection = null;
    this.selectedOption = null;
    this.memberEmail = null;

    this.identityForm.reset();
    this.responseForm.reset();
    this.passwordForm.reset();
    this.confirmationCodeForm.reset();

  }

  onRequestQuestions() {

    this.securityService.validateUserForResetPassword(this.identityForm.value.identity).subscribe(response => {
      this.memberEmail = response.email;
      this.optionsSelection = response.keys;
      this.selectedOption = this.optionsSelection[0];
    });

  }

  onRequestConfirmationCode() {

    this.securityService.getConfirmationCodeForResetPassword(this.identityForm.value.identity,
        this.selectedOption, this.responseForm.value.response).subscribe(response => {

          this.confirmationCodeForm.patchValue({confirmationCodeSent : response });

        });
  }

  onResetPassword() {

    this.securityService.resetPassword(this.memberEmail, this.passwordForm.value.password).subscribe(response => {
      this.router.navigate(['/initsession']);
    });

  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { mismatch: true };
  }

  confirmationCodeMatchValidator(g: FormGroup) {
    return g.get('confirmationCode').value === g.get('confirmationCodeSent').value ? null : { mismatch: true };
  }



}
