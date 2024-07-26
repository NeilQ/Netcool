import { NgModule } from '@angular/core';
import { SharedModule } from '@shared';
import { SysRoutingModule } from './sys-routing.module';
import { SysAppConfigComponent } from './app-config/app-config.component';
import { SysAppConfigEditComponent } from "./app-config/edit/edit.component";
import { SysOrganizationComponent } from './organization/organization.component';
import { SysOrganizationEditComponent } from './organization/edit/edit.component';
import { SysAnnouncementComponent } from './announcement/announcement.component';
import { SysAnnouncementEditComponent } from './announcement/edit/edit.component';
import { SysAnnouncementViewComponent } from './announcement/view/view.component';
import { SysUserAnnouncementComponent } from "./announcement/user-announcement/user-announcement.component";

const COMPONENTS = [
  SysAppConfigComponent,
  SysOrganizationComponent,
  SysOrganizationEditComponent,
  SysAnnouncementComponent,
  SysAnnouncementEditComponent,
  SysAnnouncementViewComponent,
  SysUserAnnouncementComponent];
const COMPONENTS_NOROUNT = [
  SysAppConfigEditComponent,
  SysOrganizationEditComponent,
  SysAnnouncementEditComponent,
  SysAnnouncementViewComponent,
  SysUserAnnouncementComponent
];

@NgModule({
    imports: [
        SharedModule,
        SysRoutingModule
    ],
    declarations: [
        ...COMPONENTS,
        ...COMPONENTS_NOROUNT,
    ]
})
export class SysModule {
}
