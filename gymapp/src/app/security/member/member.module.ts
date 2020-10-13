import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { MemberPageRoutingModule } from './member-routing.module';

import { MemberPage } from './member.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    IonicModule,
    MemberPageRoutingModule
  ],
  declarations: [MemberPage]
})
export class MemberPageModule {}
