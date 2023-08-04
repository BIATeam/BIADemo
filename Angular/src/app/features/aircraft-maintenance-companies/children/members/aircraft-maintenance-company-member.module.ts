import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AircraftMaintenanceCompanyMembersIndexComponent } from './views/aircraft-maintenance-company-members-index/aircraft-maintenance-company-members-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { AircraftMaintenanceCompanyMemberNewComponent } from './views/aircraft-maintenance-company-member-new/aircraft-maintenance-company-member-new.component';
import { AircraftMaintenanceCompanyMemberEditComponent } from './views/aircraft-maintenance-company-member-edit/aircraft-maintenance-company-member-edit.component';
import { Permission } from 'src/app/shared/permission';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { AircraftMaintenanceCompanyMemberItemComponent } from './views/aircraft-maintenance-company-member-item/aircraft-maintenance-company-member-item.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { MemberModule } from 'src/app/shared/bia-shared/feature-templates/members/member.module';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.AircraftMaintenanceCompany_Member_List_Access,
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
          permission: Permission.AircraftMaintenanceCompany_Member_Create,
          title: 'member.add',
          InjectComponent: AircraftMaintenanceCompanyMemberNewComponent,
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
        component: AircraftMaintenanceCompanyMemberItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'member.manage',
              canNavigate: true,
              permission: Permission.AircraftMaintenanceCompany_Member_Update,
              title: 'member.manage',
              InjectComponent: AircraftMaintenanceCompanyMemberEditComponent,
            },
            component: PopupLayoutComponent,
            // component: FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
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

