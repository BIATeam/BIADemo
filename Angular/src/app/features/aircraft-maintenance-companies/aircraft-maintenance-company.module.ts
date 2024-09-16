import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { ReducerManager, StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { CrudItemModule } from 'src/app/shared/bia-shared/feature-templates/crud-items/crud-item.module';
import { Permission } from 'src/app/shared/permission';
import { SharedModule } from 'src/app/shared/shared.module';
import { aircraftMaintenanceCompanyCRUDConfiguration } from './aircraft-maintenance-company.constants';
import { AircraftMaintenanceCompanyFormComponent } from './components/aircraft-maintenance-company-form/aircraft-maintenance-company-form.component';
import { AircraftMaintenanceCompanyTableComponent } from './components/aircraft-maintenance-company-table/aircraft-maintenance-company-table.component';
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
          permission: Permission.AircraftMaintenanceCompany_Create,
          title: 'aircraftMaintenanceCompany.add',
          injectComponent: AircraftMaintenanceCompanyNewComponent,
          dynamicComponent: () =>
            aircraftMaintenanceCompanyCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
        },
        component: aircraftMaintenanceCompanyCRUDConfiguration.usePopup
          ? PopupLayoutComponent
          : FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: true,
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
              injectComponent: AircraftMaintenanceCompanyEditComponent,
              dynamicComponent: () =>
                aircraftMaintenanceCompanyCRUDConfiguration.usePopup
                  ? PopupLayoutComponent
                  : FullPageLayoutComponent,
            },
            component: aircraftMaintenanceCompanyCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
          /// BIAToolKit - Begin Partial Parent AircraftMaintenanceCompany
          {
            path: 'maintenance-teams',
            data: {
              breadcrumb: 'aircraftMaintenanceCompany.maintenanceTeams',
              canNavigate: true,
              permission: Permission.MaintenanceTeam_List_Access,
            },
            loadChildren: () =>
              import(
                './children/maintenance-teams/maintenance-team.module'
              ).then(m => m.MaintenanceTeamModule),
          },
          /// BIAToolKit - End Partial Parent AircraftMaintenanceCompany
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
    AircraftMaintenanceCompanyItemComponent,
    AircraftMaintenanceCompaniesIndexComponent,
    // [Calc] : NOT used for calc (3 lines).
    // it is possible to delete unsed commponent files (views/..-new + views/..-edit + components/...-form).
    AircraftMaintenanceCompanyFormComponent,
    AircraftMaintenanceCompanyNewComponent,
    AircraftMaintenanceCompanyEditComponent,
    // [Calc] : Used only for calc it is possible to delete unsed commponent files (components/...-table)).
    AircraftMaintenanceCompanyTableComponent,
  ],
  imports: [
    SharedModule,
    CrudItemModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature(
      aircraftMaintenanceCompanyCRUDConfiguration.storeKey,
      FeatureAircraftMaintenanceCompaniesStore.reducers
    ),
    EffectsModule.forFeature([AircraftMaintenanceCompaniesEffects]),
    // TODO after creation of CRUD Team AircraftMaintenanceCompany : select the optioDto dommain module requiered for link
    // Domain Modules:
  ],
})
export class AircraftMaintenanceCompanyModule {}
