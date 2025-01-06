import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { MemberModule } from 'src/app/shared/bia-shared/feature-templates/members/member.module';
import { Permission } from 'src/app/shared/permission';
import { SharedModule } from 'src/app/shared/shared.module';
import { MaintenanceTeamMemberEditComponent } from './views/maintenance-team-member-edit/maintenance-team-member-edit.component';
import { MaintenanceTeamMemberItemComponent } from './views/maintenance-team-member-item/maintenance-team-member-item.component';
import { MaintenanceTeamMemberNewComponent } from './views/maintenance-team-member-new/maintenance-team-member-new.component';
import { MaintenanceTeamMembersIndexComponent } from './views/maintenance-team-members-index/maintenance-team-members-index.component';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.MaintenanceTeam_Member_List_Access,
      injectComponent: MaintenanceTeamMembersIndexComponent,
    },
    component: FullPageLayoutComponent,
    canActivate: [PermissionGuard],
    // [Calc] : The children are not used in calc
    children: [
      {
        path: 'create',
        data: {
          breadcrumb: 'bia.add',
          canNavigate: false,
          permission: Permission.MaintenanceTeam_Member_Create,
          title: 'member.add',
          injectComponent: MaintenanceTeamMemberNewComponent,
        },
        component: PopupLayoutComponent,
        // component: FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: true,
        },
        component: MaintenanceTeamMemberItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'member.manage',
              canNavigate: true,
              permission: Permission.MaintenanceTeam_Member_Update,
              title: 'member.manage',
              injectComponent: MaintenanceTeamMemberEditComponent,
            },
            component: PopupLayoutComponent,
            // component: FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
        ],
      },
    ],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  declarations: [
    MaintenanceTeamMemberItemComponent,
    MaintenanceTeamMembersIndexComponent,
    MaintenanceTeamMemberNewComponent,
    MaintenanceTeamMemberEditComponent,
  ],
  imports: [SharedModule, RouterModule.forChild(ROUTES), MemberModule],
})
export class MaintenanceTeamMemberModule {}
