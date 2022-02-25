import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AircraftMaintenanceCompanyMembersIndexComponent } from './views/members-index/members-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { AircraftMaintenanceCompanyMemberNewComponent } from './views/member-new/member-new.component';
import { AircraftMaintenanceCompanyMemberEditComponent } from './views/member-edit/member-edit.component';
import { Permission } from 'src/app/shared/permission';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { AircraftMaintenanceCompanyMemberItemComponent } from './views/member-item/member-item.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { MemberModule } from 'src/app/shared/bia-shared/features/members/member.module';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Member_List_Access,
      InjectComponent: AircraftMaintenanceCompanyMembersIndexComponent
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
          permission: Permission.Member_Create,
          title: 'member.add',
          InjectComponent: AircraftMaintenanceCompanyMemberNewComponent,
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
        component: AircraftMaintenanceCompanyMemberItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'member.manage',
              canNavigate: true,
              permission: Permission.Member_Update,
              title: 'member.manage',
              InjectComponent: AircraftMaintenanceCompanyMemberEditComponent,
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
    AircraftMaintenanceCompanyMemberItemComponent,
    AircraftMaintenanceCompanyMembersIndexComponent,
    AircraftMaintenanceCompanyMemberNewComponent,
    AircraftMaintenanceCompanyMemberEditComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
    MemberModule,
  ]
})
export class AircraftMaintenanceCompanyMemberModule {
}

