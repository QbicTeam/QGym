import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, EMPTY, throwError, from } from 'rxjs';
import { AlertController, LoadingController } from '@ionic/angular';

import { catchError, finalize, retryWhen, switchMap, tap } from 'rxjs/operators';

@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {

    constructor(private loadingCtrl: LoadingController, private alertCtrl: AlertController) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {


        if (request.url.indexOf('http') !== 0) {
            return next.handle(request).pipe(
                catchError(err => {
                    this.presentFailedAlert(err.error);
                    return throwError(err);
                })
            );
        }

        return from(this.loadingCtrl.create())
            .pipe(
                tap((loading) => {
                    return loading.present();
                }),
                switchMap((loading) => {
                    return next.handle(request).pipe(
                        catchError(err => {
                            this.presentFailedAlert(err.error);
                            return throwError(err);
                        }),
                        finalize(() => {
                            loading.dismiss();
                        })
                    );
                })
            );

        // console.log('intercepting... ', this.loadingCtrl.getTop());

        // this.loadingCtrl.getTop().then(hasLoading => {
        //     if (!hasLoading) {
        //         this.loadingCtrl.create({
        //             spinner: 'circular',
        //             translucent: true
        //         }).then(loading => loading.present());
        //     }
        // });

        // return next.handle(request).pipe(
        //     catchError(err => {
        //         this.presentFailedAlert(err.error);
        //         return throwError(err);
        //         // return EMPTY;
        //     }),
        //     finalize(() => {
        //         console.log('finalizando...');
        //         this.loadingCtrl.getTop().then(hasLoading => {
        //             console.log('Has Loading value: ', hasLoading, '***');
        //             if (hasLoading) {
        //                 console.log('Seems is loading...');
        //                 this.loadingCtrl.dismiss();
        //             }
        //         });
        //     })
        // );

    }

    async dissmissLoader() {
        const topLoader = await this.loadingCtrl.getTop();
        console.log('top loader: ', topLoader);
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
