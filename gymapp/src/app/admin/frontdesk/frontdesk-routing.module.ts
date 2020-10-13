import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { FrontdeskPage } from './frontdesk.page';

const routes: Routes = [
  {
    path: '',
    component: FrontdeskPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FrontdeskPageRoutingModule {}
