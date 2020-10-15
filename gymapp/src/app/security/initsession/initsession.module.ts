import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { InitsessionPageRoutingModule } from './initsession-routing.module';

import { InitsessionPage } from './initsession.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    InitsessionPageRoutingModule
  ],
  declarations: [InitsessionPage]
})
export class InitsessionPageModule {}
