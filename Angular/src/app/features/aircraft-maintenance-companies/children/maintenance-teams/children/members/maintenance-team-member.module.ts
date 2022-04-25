import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MaintenanceTeamMembersIndexComponent } from './views/maintenance-team-members-index/maintenance-team-members-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { MaintenanceTeamMemberNewComponent } from './views/maintenance-team-member-new/maintenance-team-member-new.component';
import { MaintenanceTeamMemberEditComponent } from './views/maintenance-team-member-edit/maintenance-team-member-edit.component';
import { Permission } from 'src/app/shared/permission';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { MaintenanceTeamMemberItemComponent } from './views/maintenance-team-member-item/maintenance-team-member-item.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { MemberModule } from 'src/app/shared/bia-shared/feature-templates/members/member.module';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.MaintenanceTeam_Member_List_Access,
      InjectComponent: MaintenanceTeamMembersIndexComponent
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
          InjectComponent: MaintenanceTeamMemberNewComponent,
        },
        component: PopupLayoutComponent,
        // component: FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':memberId',
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
              InjectComponent: MaintenanceTeamMemberEditComponent,
            },
            component: PopupLayoutComponent,
            // component: FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            redirectTo: 'edit'
          },
        ]
      },
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [
    MaintenanceTeamMemberItemComponent,
    MaintenanceTeamMembersIndexComponent,
    MaintenanceTeamMemberNewComponent,
    MaintenanceTeamMemberEditComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
    MemberModule,
  ]
})
export class MaintenanceTeamMemberModule {
}

