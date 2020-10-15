import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-member',
  templateUrl: './member.page.html',
  styleUrls: ['./member.page.scss'],
})
export class MemberPage implements OnInit {

  memberFound = false;
  memberName = '';
  memberEmail = '';
  optionsSelection = [];
  selectedOption;

  memberForm = new FormGroup({
    memberId: new FormControl('', Validators.required)
  });

  identityForm = new FormGroup({
    response: new FormControl('', Validators.required),
    email: new FormControl('', Validators.required)
  });

  confirmationCodeForm = this.formBuilder.group({
    confirmationCode: ['', Validators.required],
    confirmationCodeSent: [''],
    password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8) ]],
    confirmPassword: ['', Validators.required]
  }, { validators: [this.confirmationCodeMatchValidator, this.passwordMatchValidator ]});


  constructor(private formBuilder: FormBuilder, private router: Router) { }

  ngOnInit() {
  }

  onValidateMember() {

    this.memberFound = true;
    this.memberName = 'Majahide Payan Hz';

    const rnd = Math.floor(Math.random() * Math.floor(2));
    console.log('Validating member...', rnd);

    if (rnd === 0) {

      this.memberEmail = '';
      this.optionsSelection = [ 'Telefono', 'Paquete', 'Edad'];
      this.selectedOption = this.optionsSelection[0];

    } else {

      this.memberEmail = 'maj***********@hotmail.com';
      this.confirmationCodeForm.patchValue({confirmationCodeSent : 12345 });
    }
  }

  onIdentityConfirmation() {
    console.log('identity validation...');
    this.memberEmail = 'maj***********@hotmail.com';

    this.confirmationCodeForm.patchValue({confirmationCodeSent : 12345 });


  }

  confirmationCodeMatchValidator(g: FormGroup) {
    return g.get('confirmationCode').value === g.get('confirmationCodeSent').value ? null : { mismatchCode: true };
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { mismatch: true };
  }

  onRegisterMember() {
    this.router.navigate(['/schedule']);
  }
}
