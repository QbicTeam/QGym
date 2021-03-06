import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';
import { FileUploadModule } from 'ng2-file-upload';

import { ConfigurationPageRoutingModule } from './configuration-routing.module';

import { ConfigurationPage } from './configuration.page';
import { BlockusersComponent } from './blockusers/blockusers.component';
import { BlockModalComponent } from './block-modal/block-modal.component';
import { CapacityComponent } from './capacity/capacity.component';
import { CapacityModalComponent } from './capacity-modal/capacity-modal.component';
import { ScheduleComponent } from './schedule/schedule.component';
import { UsersComponent } from './users/users.component';
import { UsersModalComponent } from './users-modal/users-modal.component';
import { GympackagesComponent } from './gympackages/gympackages.component';
import { PackagesModalComponent } from './packages-modal/packages-modal.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    FileUploadModule,
    ConfigurationPageRoutingModule
  ],
  declarations: [ConfigurationPage, BlockusersComponent, BlockModalComponent, CapacityComponent,
    CapacityModalComponent, ScheduleComponent, UsersComponent, UsersModalComponent,
    GympackagesComponent, PackagesModalComponent]
})
export class ConfigurationPageModule {}
