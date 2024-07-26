import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { environment } from '@env/environment';
// layout
import { LayoutPassportComponent } from '../layout/passport/passport.component';
// dashboard pages
import { DashboardComponent } from './dashboard/dashboard.component';
// passport pages
import { UserLoginComponent } from './passport/login/login.component';
// single pages
import { CallbackComponent } from './callback/callback.component';
import { UserLockComponent } from './passport/lock/lock.component';
import { authSimpleCanActivate, authSimpleCanActivateChild } from '@delon/auth';
import { LayoutBasicComponent } from "../layout/basic/basic.component";
import { PreloadOptionalModules } from '@delon/theme';

const routes: Routes = [
  {
    path: '',
    component: LayoutBasicComponent,
    canActivate: [ authSimpleCanActivate],
    canActivateChild: [authSimpleCanActivateChild],
    children: [
      {path: '', redirectTo: 'dashboard', pathMatch: 'full'},
      {path: 'dashboard', component: DashboardComponent, data: {title: '仪表盘', titleI18n: 'dashboard'}},
      {path: 'exception', loadChildren: () => import('./exception/exception.module').then(m => m.ExceptionModule)},
      // 业务子模块
      {path: 'system', loadChildren: () => import('./sys/sys.module').then(m => m.SysModule)},
      {path: 'auth', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)},
    ]
  },
  // 全屏布局
  // {
  //     path: 'fullscreen',
  //     component: LayoutFullScreenComponent,
  //     children: [
  //     ]
  // },
  // passport
  {
    path: 'passport',
    component: LayoutPassportComponent,
    children: [
      {path: 'login', component: UserLoginComponent, data: {title: '登录'}},
      {path: 'lock', component: UserLockComponent, data: {title: '锁屏'}},
    ]
  },
  // 单页不包裹Layout
  {path: 'callback/:type', component: CallbackComponent},
  {path: '**', redirectTo: 'exception/404'},
];

@NgModule({
  providers: [PreloadOptionalModules],
  imports: [
    RouterModule.forRoot(
      routes, {
        useHash: environment.useHash,
        // NOTICE: If you use `reuse-tab` component and turn on keepingScroll you can set to `disabled`
        // Pls refer to https://ng-alain.com/components/reuse-tab
        scrollPositionRestoration: 'top',
        preloadingStrategy: PreloadOptionalModules,
        bindToComponentInputs: true
      }
    )],
  exports: [RouterModule],
})
export class RouteRoutingModule {
}
