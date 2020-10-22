import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { EMPTY, Observable } from 'rxjs';
import { UserValidationResponseDTO } from '../_models/UserValidationResponseDTO';
import { MemberDataForRegisterDTO } from '../_models/MemberDataForRegisterDTO';

@Injectable({
  providedIn: 'root'
})
export class SecurityService {

  baseUrl = environment.apiurl;
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient, private router: Router) { }

  getConfirmationCodeForRegister(email: string) {

    const url = this.baseUrl + 'auth/register/confirmationCode';
    const requestData = {
      email
    };

    return this.http.post(url, requestData);

  }

  register(email: string, displayName: string, password: string) {

    const url = this.baseUrl + 'auth/register';

    const requestData = {
      userName: email,
      displayName,
      password
    };

    return this.http.post(url, requestData);

  }

  login(email: string, password: string) {

    const url = this.baseUrl + 'auth/login';

    const requestData = {
      userName: email,
      password
    };

    return this.http.post(url, requestData)
      .pipe(map((response: any) => {

        this.decodedToken = this.jwtHelper.decodeToken(response.token);

        const userData = {
          id: + this.decodedToken.nameid,
          email: this.decodedToken.email,
          displayName: this.decodedToken.unique_name,
          role: response.role,
          expiration: response.membershipExpiration,
          photoUrl: response.photoUrl,
          packageId: response.packageId
        };

        localStorage.setItem('gymToken', response.token);
        localStorage.setItem('gymUserData', JSON.stringify(userData));

      }));

  }

  validateUserForResetPassword(memberId: string): Observable<UserValidationResponseDTO> {

    const url = this.baseUrl + 'auth/resetpassword/validation';

    const requestData = {
      memberId
    };

    return this.http.post<UserValidationResponseDTO>(url, requestData);

  }

  getConfirmationCodeForResetPassword(memberId, key, value) {

    const url = this.baseUrl + 'auth/resetpassword/confirmation';

    const requestData = {
      memberId,
      key,
      value
    };

    return this.http.post(url, requestData);

  }

  resetPassword(email, newPassword) {

    const url = this.baseUrl + 'auth/resetpassword';

    const requestData = {
      email,
      newPassword
    };

    return this.http.put(url, requestData);

  }


  getUserDataForRegisterByMemberId(memberId): Observable<MemberDataForRegisterDTO> {

    const url = this.baseUrl + 'auth/member/' + memberId;

    return this.http.get<MemberDataForRegisterDTO>(url);
  }


  confirmIdentityForRegister(memberId, email, key, value) {

    const url = this.baseUrl + 'auth/confirmationCode';

    const requestData = {
      memberId,
      email,
      key,
      value
    };

    return this.http.post(url, requestData);
  }

  registerCurrentMember(memberId, email, password) {

    const url = this.baseUrl + 'auth/register';

    const requestData = {
      memberId,
      email,
      password
    };

    return this.http.put(url, requestData);

  }

  getCurrentLoggedUser() {

     const result = JSON.parse(localStorage.getItem('gymUserData'));

     return result;
  }

  checkExpiration(): boolean {

    let result = false;

    const currentUser = this.getCurrentLoggedUser();
    const dueDate = new Date(currentUser.expiration);
    const today = new Date();

    if (dueDate >= today) {
       result = true;
    } else {
      if (currentUser.packageId === 0) {
        this.router.navigate(['/packages']);
      } else {
        this.router.navigate(['/payment']);
      }

    }

    return result;
  }

  getMenuByCurrentUserRole() {

    const user = this.getCurrentLoggedUser();

    let result: any;

    if (user.role.toLowerCase() === 'admin') {
      result = [ { icon: 'alarm-outline', displayName: 'Mostrador', option: 'frontdesk' },
                  { icon: 'cog-outline', displayName: 'Administrador', option: 'admin' },
                  { icon: 'cash-outline', displayName: 'Reporte de Ventas', option: 'sales' },
                  { icon: 'cash-outline', displayName: 'Activacion de Membresias', option: 'sales' } ];
    } else {
      result = [ { icon: 'albums-outline', displayName: 'Paquetes', option: 'packages' },
              { icon: 'alarm-outline', displayName: 'Agenda', option: 'schedule' } ];
    }

    return result;
  }

}
