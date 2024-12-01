import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SysAppConfigComponent } from './app-config/app-config.component';
import { SysOrganizationComponent } from './organization/organization.component';
import { SysAnnouncementComponent } from './announcement/announcement.component';

const routes: Routes = [
  {path: 'app-configuration', component: SysAppConfigComponent},
  {path: 'organization', component: SysOrganizationComponent},
  {path: 'announcement', component: SysAnnouncementComponent}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SysRoutingModule {
}
