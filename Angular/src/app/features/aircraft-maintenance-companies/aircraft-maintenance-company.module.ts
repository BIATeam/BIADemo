import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'packages/bia-ng/core/public-api';
import {
  DynamicLayoutComponent,
  LayoutMode,
} from 'packages/bia-ng/shared/public-api';
import { Permission } from 'src/app/shared/permission';
import { aircraftMaintenanceCompanyCRUDConfiguration } from './aircraft-maintenance-company.constants';
import { AircraftMaintenanceCompaniesEffects } from './store/aircraft-maintenance-companies-effects';
import { FeatureAircraftMaintenanceCompaniesStore } from './store/aircraft-maintenance-company.state';
import { AircraftMaintenanceCompaniesIndexComponent } from './views/aircraft-maintenance-companies-index/aircraft-maintenance-companies-index.component';
import { AircraftMaintenanceCompanyEditComponent } from './views/aircraft-maintenance-company-edit/aircraft-maintenance-company-edit.component';
import { AircraftMaintenanceCompanyItemComponent } from './views/aircraft-maintenance-company-item/aircraft-maintenance-company-item.component';
import { AircraftMaintenanceCompanyNewComponent } from './views/aircraft-maintenance-company-new/aircraft-maintenance-company-new.component';

export const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.AircraftMaintenanceCompany_List_Access,
      injectComponent: AircraftMaintenanceCompaniesIndexComponent,
      configuration: aircraftMaintenanceCompanyCRUDConfiguration,
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
          permission: Permission.AircraftMaintenanceCompany_Create,
          title: 'aircraftMaintenanceCompany.add',
        },
        component: AircraftMaintenanceCompanyNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: false,
        },
        component: AircraftMaintenanceCompanyItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'members',
            data: {
              breadcrumb: 'app.members',
              canNavigate: true,
              permission:
                Permission.AircraftMaintenanceCompany_Member_List_Access,
              layoutMode: LayoutMode.fullPage,
            },
            loadChildren: () =>
              import(
                './children/members/aircraft-maintenance-company-member.module'
              ).then(m => m.AircraftMaintenanceCompanyMemberModule),
          },
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.AircraftMaintenanceCompany_Update,
              title: 'aircraftMaintenanceCompany.edit',
            },
            component: AircraftMaintenanceCompanyEditComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
          // BIAToolKit - Begin AircraftMaintenanceCompanyModuleChildPath
          // Begin BIAToolKit Generation Ignore
          // BIAToolKit - Begin Partial AircraftMaintenanceCompanyModuleChildPath MaintenanceTeam
          {
            path: 'maintenance-teams',
            data: {
              breadcrumb: 'app.maintenanceTeams',
              canNavigate: true,
              permission: Permission.MaintenanceTeam_List_Access,
              layoutMode: LayoutMode.fullPage,
            },
            loadChildren: () =>
              import(
                './children/maintenance-teams/maintenance-team.module'
              ).then(m => m.MaintenanceTeamModule),
          },
          // BIAToolKit - End Partial AircraftMaintenanceCompanyModuleChildPath MaintenanceTeam
          // End BIAToolKit Generation Ignore
          // BIAToolKit - End AircraftMaintenanceCompanyModuleChildPath
        ],
      },
    ],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature(
      aircraftMaintenanceCompanyCRUDConfiguration.storeKey,
      FeatureAircraftMaintenanceCompaniesStore.reducers
    ),
    EffectsModule.forFeature([AircraftMaintenanceCompaniesEffects]),
    // Domain Modules:
  ],
})
export class AircraftMaintenanceCompanyModule {}
