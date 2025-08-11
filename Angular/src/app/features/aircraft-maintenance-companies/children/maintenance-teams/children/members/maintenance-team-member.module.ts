import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PermissionGuard } from 'packages/bia-ng/core/public-api';
import {
  FullPageLayoutComponent,
  MemberModule,
  PopupLayoutComponent,
} from 'packages/bia-ng/shared/public-api';
import { Permission } from 'src/app/shared/permission';

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
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: false,
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
  imports: [RouterModule.forChild(ROUTES), MemberModule],
})
export class MaintenanceTeamMemberModule {}
