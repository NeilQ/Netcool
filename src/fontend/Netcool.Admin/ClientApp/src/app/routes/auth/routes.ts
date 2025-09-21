import { Routes } from '@angular/router';
import { AuthRoleComponent } from './role/role.component';
import { AuthUserComponent } from "./user/user.component";
import {AuthMenuComponent} from "./menu/menu.component";

export const routes: Routes = [
  {path: 'role', component: AuthRoleComponent},
  {path: 'user', component: AuthUserComponent},
  {path: 'menu', component: AuthMenuComponent}
];

