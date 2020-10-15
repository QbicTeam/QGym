import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { FrontdeskmanagerPage } from './frontdeskmanager.page';

const routes: Routes = [
  {
    path: '',
    component: FrontdeskmanagerPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FrontdeskmanagerPageRoutingModule {}
