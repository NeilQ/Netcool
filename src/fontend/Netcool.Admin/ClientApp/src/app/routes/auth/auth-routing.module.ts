import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthRoleComponent } from './role/role.component';
import { AuthUserComponent } from "./user/user.component";
import {AuthMenuComponent} from "./menu/menu.component";

const routes: Routes = [
  {path: 'role', component: AuthRoleComponent},
  {path: 'user', component: AuthUserComponent},
  {path: 'menu', component: AuthMenuComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule {
}
