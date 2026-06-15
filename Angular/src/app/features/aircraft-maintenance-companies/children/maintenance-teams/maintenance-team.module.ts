import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'packages/bia-ng/core/public-api';
import {
  DynamicLayoutComponent,
  LayoutMode,
} from 'packages/bia-ng/shared/public-api';
import { AirportOptionModule } from 'src/app/domains/airport-option/airport-option.module';
import { CountryOptionModule } from 'src/app/domains/country-option/country-option.module';
import { Permission } from 'src/app/shared/permission';
import { aircraftMaintenanceCompanyCRUDConfiguration } from '../../aircraft-maintenance-company.constants';
import { AircraftMaintenanceCompaniesEffects } from '../../store/aircraft-maintenance-companies-effects';
import { FeatureAircraftMaintenanceCompaniesStore } from '../../store/aircraft-maintenance-company.state';
import { maintenanceTeamCRUDConfiguration } from './maintenance-team.constants';
import { MaintenanceTeamService } from './services/maintenance-team.service';
import { FeatureMaintenanceTeamsStore } from './store/maintenance-team.state';
import { MaintenanceTeamsEffects } from './store/maintenance-teams-effects';
import { MaintenanceTeamEditComponent } from './views/maintenance-team-edit/maintenance-team-edit.component';
import { MaintenanceTeamItemComponent } from './views/maintenance-team-item/maintenance-team-item.component';
import { MaintenanceTeamNewComponent } from './views/maintenance-team-new/maintenance-team-new.component';
import { MaintenanceTeamsIndexComponent } from './views/maintenance-teams-index/maintenance-teams-index.component';

export const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.MaintenanceTeam_List_Access,
      injectComponent: MaintenanceTeamsIndexComponent,
      configuration: maintenanceTeamCRUDConfiguration,
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
          permission: Permission.MaintenanceTeam_Create,
          title: 'maintenanceTeam.add',
        },
        component: MaintenanceTeamNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: 'view',
        data: {
          featureConfiguration: maintenanceTeamCRUDConfiguration,
          featureServiceType: MaintenanceTeamService,
          leftWidth: 60,
        },
        loadChildren: () =>
          import('../../../../shared/bia-shared/view.module').then(
            m => m.ViewModule
          ),
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: false,
        },
        component: MaintenanceTeamItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'members',
            data: {
              breadcrumb: 'app.members',
              canNavigate: true,
              permission: Permission.MaintenanceTeam_Member_List_Access,
              layoutMode: LayoutMode.fullPage,
            },
            loadChildren: () =>
              import('./children/members/maintenance-team-member.module').then(
                m => m.MaintenanceTeamMemberModule
              ),
          },
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.MaintenanceTeam_Update,
              title: 'maintenanceTeam.edit',
            },
            component: MaintenanceTeamEditComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
          // BIAToolKit - Begin MaintenanceTeamModuleChildPath
          // BIAToolKit - End MaintenanceTeamModuleChildPath
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
      maintenanceTeamCRUDConfiguration.storeKey,
      FeatureMaintenanceTeamsStore.reducers
    ),
    // Team Parent Store:
    StoreModule.forFeature(
      aircraftMaintenanceCompanyCRUDConfiguration.storeKey,
      FeatureAircraftMaintenanceCompaniesStore.reducers
    ),
    EffectsModule.forFeature([
      MaintenanceTeamsEffects,
      AircraftMaintenanceCompaniesEffects,
    ]),
    // Domain Modules:
    AirportOptionModule,
    CountryOptionModule,
  ],
})
export class MaintenanceTeamModule {}
