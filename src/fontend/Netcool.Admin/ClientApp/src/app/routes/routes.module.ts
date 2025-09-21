import { NgModule } from '@angular/core';

import { RouteRoutingModule } from './routes-routing.module';
// dashboard pages
import { DashboardComponent } from './dashboard/dashboard.component';
// passport pages
import { UserLoginComponent } from './passport/login/login.component';
// single pages
import { UserLockComponent } from './passport/lock/lock.component';

const COMPONENTS = [
  DashboardComponent,
  // passport pages
  UserLoginComponent,
  // single pages
  UserLockComponent,
];

@NgModule({
  imports: [ RouteRoutingModule ],
  declarations: [
    //...COMPONENTS,
  ]
})
export class RoutesModule {}
