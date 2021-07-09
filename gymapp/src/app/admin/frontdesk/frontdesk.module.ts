import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { FrontdeskPageRoutingModule } from './frontdesk-routing.module';

import { FrontdeskPage } from './frontdesk.page';
import { Router } from '@angular/router';
import { SecurityService } from 'src/app/api/security.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    FrontdeskPageRoutingModule
  ],
  declarations: [FrontdeskPage]
})
export class FrontdeskPageModule {

  constructor( private router: Router, private securityService: SecurityService) { }

  logOut() {
    this.securityService.logOut();
    this.router.navigateByUrl('/home');
  }


}
