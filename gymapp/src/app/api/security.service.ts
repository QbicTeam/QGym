import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SecurityService {

  baseUrl = environment.apiurl;

  constructor(private http: HttpClient) { }

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
        console.log('user logged: ', response);



        const userData = {
          role: response.role,
          expiration: response.membershipExpiration,
          photoUrl: response.photoUrl
        };
      }));

  }

}


/*

    return this._http.post(this.baseUrl + 'login', model)
      .pipe(map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', JSON.stringify(user.user));
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.currentUser = user.user;
          this.changeMemberPhoto(this.currentUser.photoUrl);
        }
      }));
*/