import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { SchedulePageRoutingModule } from './schedule-routing.module';

import { SchedulePage } from './schedule.page';
import { CovidalertComponent } from '../covidalert/covidalert.component';
import { SchedulesummaryComponent } from '../schedulesummary/schedulesummary.component';
import { CheckinComponent } from '../checkin/checkin.component';
import { ProfileComponent } from '../profile/profile.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    SchedulePageRoutingModule
  ],
  declarations: [SchedulePage, CovidalertComponent, SchedulesummaryComponent, CheckinComponent, ProfileComponent]
})
export class SchedulePageModule {}
