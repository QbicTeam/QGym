import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MembersactivationPage } from './membersactivation.page';

const routes: Routes = [
  {
    path: '',
    component: MembersactivationPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MembersactivationPageRoutingModule {}
