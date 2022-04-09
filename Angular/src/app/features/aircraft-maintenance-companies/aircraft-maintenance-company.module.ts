import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { AircraftMaintenanceCompaniesEffects } from './store/aircraft-maintenance-companies-effects';
import { reducers } from './store/aircraft-maintenance-company.state';
import { AircraftMaintenanceCompanyFormComponent } from './components/aircraft-maintenance-company-form/aircraft-maintenance-company-form.component';
import { AircraftMaintenanceCompaniesIndexComponent } from './views/aircraft-maintenance-companies-index/aircraft-maintenance-companies-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { AircraftMaintenanceCompanyNewComponent } from './views/aircraft-maintenance-company-new/aircraft-maintenance-company-new.component';
import { AircraftMaintenanceCompanyEditComponent } from './views/aircraft-maintenance-company-edit/aircraft-maintenance-company-edit.component';
import { Permission } from 'src/app/shared/permission';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { AircraftMaintenanceCompanyItemComponent } from './views/aircraft-maintenance-company-item/aircraft-maintenance-company-item.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { AircraftMaintenanceCompanyTableComponent } from './components/aircraft-maintenance-company-table/aircraft-maintenance-company-table.component';
import { storeKey, usePopup } from './aircraft-maintenance-company.constants';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.AircraftMaintenanceCompany_List_Access,
      InjectComponent: AircraftMaintenanceCompaniesIndexComponent
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
          InjectComponent: AircraftMaintenanceCompanyNewComponent,
        },
        component: (usePopup) ? PopupLayoutComponent : FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':aircraftMaintenanceCompanyId',
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
              permission: Permission.AircraftMaintenanceCompany_Member_List_Access
            },
            loadChildren: () =>
              import('./children/members/aircraft-maintenance-company-member.module').then((m) => m.AircraftMaintenanceCompanyMemberModule)
          },
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.AircraftMaintenanceCompany_Update,
              title: 'aircraftMaintenanceCompany.edit',
              InjectComponent: AircraftMaintenanceCompanyEditComponent,
            },
            component: (usePopup) ? PopupLayoutComponent : FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: 'maintenance-teams',
            data: {
              breadcrumb: 'aircraftMaintenanceCompany.maintenanceTeams',
              canNavigate: true,
              permission: Permission.MaintenanceTeam_List_Access
            },
            loadChildren: () =>
              import('./children/maintenance-teams/maintenance-team.module').then((m) => m.MaintenanceTeamModule)
          },
        ]
      },
    ]
  },
  { path: '**', redirectTo: '' }
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
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature(storeKey, reducers),
    EffectsModule.forFeature([AircraftMaintenanceCompaniesEffects]),
    // Domain Modules:
  ]
})

export class AircraftMaintenanceCompanyModule {
}

