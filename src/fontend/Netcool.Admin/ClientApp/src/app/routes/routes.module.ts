import { NgModule } from '@angular/core';

import { SharedModule } from '@shared';
import { RouteRoutingModule } from './routes-routing.module';
// dashboard pages
import { DashboardComponent } from './dashboard/dashboard.component';
// passport pages
import { UserLoginComponent } from './passport/login/login.component';
// single pages
import { CallbackComponent } from './callback/callback.component';
import { UserLockComponent } from './passport/lock/lock.component';

const COMPONENTS = [
  DashboardComponent,
  // passport pages
  UserLoginComponent,
  // single pages
  CallbackComponent,
  UserLockComponent,
];

@NgModule({
  imports: [ SharedModule, RouteRoutingModule ],
  declarations: [
    ...COMPONENTS,
  ]
})
export class RoutesModule {}
