import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'home',
    loadChildren: () => import('./home/home.module').then( m => m.HomePageModule)
  },
  {
    path: 'initsession',
    loadChildren: () => import('./security/initsession/initsession.module').then( m => m.InitsessionPageModule)
  },
  {
    path: 'login',
    loadChildren: () => import('./security/login/login.module').then( m => m.LoginPageModule)
  },
  {
    path: 'member',
    loadChildren: () => import('./security/member/member.module').then( m => m.MemberPageModule)
  },
  {
    path: 'register',
    loadChildren: () => import('./security/register/register.module').then(m => m.RegisterPageModule)
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'resetpwd',
    loadChildren: () => import('./security/resetpwd/resetpwd.module').then( m => m.ResetpwdPageModule)
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./main/dashboard/dashboard.module').then( m => m.DashboardPageModule)
  },
  {
    path: 'configuration',
    loadChildren: () => import('./admin/configuration/configuration.module').then( m => m.ConfigurationPageModule)
  },
  {
    path: 'schedule',
    loadChildren: () => import('./member/schedule/schedule.module').then( m => m.SchedulePageModule)
  },
  {
    path: 'frontdesk',
    loadChildren: () => import('./admin/frontdesk/frontdesk.module').then( m => m.FrontdeskPageModule)
  },
  {
    path: 'frontdeskmanager',
    loadChildren: () => import('./admin/frontdeskmanager/frontdeskmanager.module').then( m => m.FrontdeskmanagerPageModule)
  },
  {
    path: 'packages',
    loadChildren: () => import('./pkgs/packages/packages.module').then( m => m.PackagesPageModule)
  },
  {
    path: 'package-detail',
    loadChildren: () => import('./pkgs/package-detail/package-detail.module').then( m => m.PackageDetailPageModule)
  },
  {
    path: 'payment',
    loadChildren: () => import('./pkgs/payment/payment.module').then( m => m.PaymentPageModule)
  },
  {
    path: 'sales-report',
    loadChildren: () => import('./admin/sales-report/sales-report.module').then( m => m.SalesReportPageModule)
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
