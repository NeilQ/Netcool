import { Routes } from '@angular/router';
import { SysAppConfigComponent } from './app-config/app-config.component';
import { SysOrganizationComponent } from './organization/organization.component';
import { SysAnnouncementComponent } from './announcement/announcement.component';

export const routes: Routes = [
  {path: 'app-configuration', component: SysAppConfigComponent},
  {path: 'organization', component: SysOrganizationComponent},
  {path: 'announcement', component: SysAnnouncementComponent}];

