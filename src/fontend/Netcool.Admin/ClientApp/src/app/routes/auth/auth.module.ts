import { NgModule } from '@angular/core';
import { SharedModule } from '@shared';
import { AuthRoutingModule } from './auth-routing.module';
import { AuthRoleComponent } from './role/role.component';
import { AuthRoleEditComponent } from './role/edit/edit.component';
import { AuthUserComponent } from "./user/user.component";
import { AuthUserEditComponent } from "./user/edit/edit.component";
import { AuthUserRoleEditComponent } from "./user/edit-role/edit-role.component";
import { AuthRoleSetPermissionsComponent } from './role/set-permissions/set-permissions.component';
import { AuthUserResetPasswordComponent } from './user/reset-password/reset-password.component';
import { AuthMenuComponent } from "./menu/menu.component";

const COMPONENTS = [
  AuthRoleComponent,
  AuthUserComponent,
  AuthMenuComponent
];
const COMPONENTS_NOROUNT = [
  AuthRoleEditComponent,
  AuthUserEditComponent,
  AuthUserRoleEditComponent,
  AuthRoleSetPermissionsComponent,
  AuthUserResetPasswordComponent];

@NgModule({
    imports: [
        SharedModule,
        AuthRoutingModule
    ],
    declarations: [
        ...COMPONENTS,
        ...COMPONENTS_NOROUNT
    ]
})
export class AuthModule {
}
