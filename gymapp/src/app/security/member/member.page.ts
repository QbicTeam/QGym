import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { SecurityService } from 'src/app/api/security.service';

@Component({
  selector: 'app-member',
  templateUrl: './member.page.html',
  styleUrls: ['./member.page.scss'],
})
export class MemberPage implements OnInit {

  memberFound = false;
  memberName = '';
  memberEmail = '';
  fullMemberEmail = '';
  optionsSelection = [];
  selectedOption;

  memberForm = new FormGroup({
    memberId: new FormControl('', Validators.required)
  });

  identityForm = new FormGroup({
    response: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required,
      Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')])
  });

  confirmationCodeForm = this.formBuilder.group({
    confirmationCode: ['', Validators.required],
    confirmationCodeSent: [''],
    password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8) ]],
    confirmPassword: ['', Validators.required]
  }, { validators: [this.confirmationCodeMatchValidator, this.passwordMatchValidator ]});


  constructor(private formBuilder: FormBuilder, private router: Router, private securityService: SecurityService) { }

  ngOnInit() {
  }

  onValidateMember() {

    this.securityService.getUserDataForRegisterByMemberId(this.memberForm.value.memberId).subscribe(response => {

      this.memberFound = true;
      this.memberName = response.displayName;

      if (!response.email) {

        this.memberEmail = '';
        this.optionsSelection = response.validationTypes;
        this.selectedOption = this.optionsSelection[0];

      } else {

        this.memberEmail = response.emailObfuscated;
        this.fullMemberEmail = response.email;
        this.confirmationCodeForm.patchValue({confirmationCodeSent : response.confirmationCode });

      }

    });

  }

  onIdentityConfirmation() {


    this.securityService.confirmIdentityForRegister(this.memberForm.value.memberId,
      this.identityForm.value.email, this.selectedOption, this.identityForm.value.response). subscribe(response => {

        if (!this.identityForm.value.email || this.identityForm.value.email !== '') {
          this.memberEmail = this.identityForm.value.email;
          this.fullMemberEmail = this.memberEmail;
        }

        this.confirmationCodeForm.patchValue({confirmationCodeSent : response });

      });

  }

  confirmationCodeMatchValidator(g: FormGroup) {
    return g.get('confirmationCode').value === g.get('confirmationCodeSent').value ? null : { mismatchCode: true };
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { mismatch: true };
  }

  onRegisterMember() {

    if (!this.memberEmail || this.memberEmail === '') {
      this.securityService.registerCurrentMember(this.memberForm.value.memberId, this.fullMemberEmail,
        this.confirmationCodeForm.value.password).subscribe(response => {

          this.login(this.fullMemberEmail, this.confirmationCodeForm.value.password);

        });
    } else {

      this.securityService.resetPassword(this.fullMemberEmail, this.confirmationCodeForm.value.password).subscribe(response => {

        this.login(this.fullMemberEmail, this.confirmationCodeForm.value.password);

      });

    }
  }

  login(email, password) {

    this.securityService.login(email, password).subscribe(response => {

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

}
