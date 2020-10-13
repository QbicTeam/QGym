import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { FrontdeskPageRoutingModule } from './frontdesk-routing.module';

import { FrontdeskPage } from './frontdesk.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    FrontdeskPageRoutingModule
  ],
  declarations: [FrontdeskPage]
})
export class FrontdeskPageModule {}
