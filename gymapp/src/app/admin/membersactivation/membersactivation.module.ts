import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { MembersactivationPageRoutingModule } from './membersactivation-routing.module';

import { MembersactivationPage } from './membersactivation.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    MembersactivationPageRoutingModule
  ],
  declarations: [MembersactivationPage]
})
export class MembersactivationPageModule {}
