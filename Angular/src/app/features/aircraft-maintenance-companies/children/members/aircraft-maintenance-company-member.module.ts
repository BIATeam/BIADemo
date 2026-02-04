import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PermissionGuard } from '@bia-team/bia-ng/core';
import {
  DynamicLayoutComponent,
  LayoutMode,
  MemberModule,
} from '@bia-team/bia-ng/shared';
import { Permission } from 'src/app/shared/permission';

import { AircraftMaintenanceCompanyMemberEditComponent } from './views/aircraft-maintenance-company-member-edit/aircraft-maintenance-company-member-edit.component';
import { AircraftMaintenanceCompanyMemberItemComponent } from './views/aircraft-maintenance-company-member-item/aircraft-maintenance-company-member-item.component';
import { AircraftMaintenanceCompanyMemberNewComponent } from './views/aircraft-maintenance-company-member-new/aircraft-maintenance-company-member-new.component';
import { AircraftMaintenanceCompanyMembersIndexComponent } from './views/aircraft-maintenance-company-members-index/aircraft-maintenance-company-members-index.component';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.AircraftMaintenanceCompany_Member_List_Access,
      injectComponent: AircraftMaintenanceCompanyMembersIndexComponent,
    },
    component: DynamicLayoutComponent,
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
          layoutMode: LayoutMode.popup,
        },
        component: AircraftMaintenanceCompanyMemberNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: false,
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
              layoutMode: LayoutMode.popup,
            },
            component: AircraftMaintenanceCompanyMemberEditComponent,
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
export class AircraftMaintenanceCompanyMemberModule {}
