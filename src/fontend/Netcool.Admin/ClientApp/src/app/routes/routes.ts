import { Routes } from "@angular/router";
import { LayoutBasicComponent } from "../layout/basic/basic.component";
import { authSimpleCanActivate, authSimpleCanActivateChild } from "@delon/auth";
import { DashboardComponent } from "./dashboard/dashboard.component";

export const routes: Routes = [
  {
    path: '',
    component: LayoutBasicComponent,
    canActivate: [authSimpleCanActivate],
    canActivateChild: [authSimpleCanActivateChild],
    children: [
      {path: '', redirectTo: 'dashboard', pathMatch: 'full'},
      {path: 'dashboard', component: DashboardComponent},
      {path: 'exception', loadChildren: () => import('./exception/routes').then(m => m.routes)},
      // 业务子模块
      {path: 'system', loadChildren: () => import('./sys/routes').then(m => m.routes)},
      {path: 'auth', loadChildren: () => import('./auth/routes').then(m => m.routes)},
    ]
  },
  // passport
  {path: '', loadChildren: () => import('./passport/routes').then(m => m.routes)},
  {path: '**', redirectTo: 'exception/404'},
];
