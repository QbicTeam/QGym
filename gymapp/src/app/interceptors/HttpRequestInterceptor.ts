import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, EMPTY } from 'rxjs';
import { AlertController, LoadingController } from '@ionic/angular';

import { catchError, finalize, retryWhen } from 'rxjs/operators';

@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {

    constructor(private loadingCtrl: LoadingController, private alertCtrl: AlertController) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {


        this.loadingCtrl.getTop().then(hasLoading => {
            if (!hasLoading) {
                this.loadingCtrl.create({
                    spinner: 'circular',
                    translucent: true
                }).then(loading => loading.present());
            }
        });

        return next.handle(request).pipe(
            catchError(err => {
                this.presentFailedAlert(err.error);
                return EMPTY;
            }),
            finalize(() => {
                this.loadingCtrl.getTop().then(hasLoading => {
                    if (hasLoading) {
                        this.loadingCtrl.dismiss();
                    }
                });
            })
        );

    }

    async presentFailedAlert(msg) {
        const alert = this.alertCtrl.create({
            header: 'Error:',
            message: msg,
            buttons: ['Ok']
        });

        (await alert).present();
    }

}
