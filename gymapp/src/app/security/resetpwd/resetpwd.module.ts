import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ResetpwdPageRoutingModule } from './resetpwd-routing.module';

import { ResetpwdPage } from './resetpwd.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    IonicModule,
    ResetpwdPageRoutingModule
  ],
  declarations: [ResetpwdPage]
})
export class ResetpwdPageModule {}
