import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { ReducerManager, StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
// BIAToolKit - Begin Option Airport
import { AirportOptionModule } from 'src/app/domains/airport-option/airport-option.module';
// BIAToolKit - End Option Airport
// BIAToolKit - Begin Option Country
import { CountryOptionModule } from 'src/app/domains/country-option/country-option.module';
// BIAToolKit - End Option Country
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { CrudItemModule } from 'src/app/shared/bia-shared/feature-templates/crud-items/crud-item.module';
import { Permission } from 'src/app/shared/permission';
import { SharedModule } from 'src/app/shared/shared.module';
import { MaintenanceTeamFormComponent } from './components/maintenance-team-form/maintenance-team-form.component';
import { MaintenanceTeamTableComponent } from './components/maintenance-team-table/maintenance-team-table.component';
import { maintenanceTeamCRUDConfiguration } from './maintenance-team.constants';
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
          permission: Permission.MaintenanceTeam_Create,
          title: 'maintenanceTeam.add',
          injectComponent: MaintenanceTeamNewComponent,
          dynamicComponent: () =>
            maintenanceTeamCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
        },
        component: maintenanceTeamCRUDConfiguration.usePopup
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
        component: MaintenanceTeamItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'members',
            data: {
              breadcrumb: 'app.members',
              canNavigate: true,
              permission: Permission.MaintenanceTeam_Member_List_Access,
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
              injectComponent: MaintenanceTeamEditComponent,
              dynamicComponent: () =>
                maintenanceTeamCRUDConfiguration.usePopup
                  ? PopupLayoutComponent
                  : FullPageLayoutComponent,
            },
            component: maintenanceTeamCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
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
    MaintenanceTeamItemComponent,
    MaintenanceTeamsIndexComponent,
    // [Calc] : NOT used for calc (3 lines).
    // it is possible to delete unsed commponent files (views/..-new + views/..-edit + components/...-form).
    MaintenanceTeamFormComponent,
    MaintenanceTeamNewComponent,
    MaintenanceTeamEditComponent,
    // [Calc] : Used only for calc it is possible to delete unsed commponent files (components/...-table)).
    MaintenanceTeamTableComponent,
  ],
  imports: [
    SharedModule,
    CrudItemModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature(
      maintenanceTeamCRUDConfiguration.storeKey,
      FeatureMaintenanceTeamsStore.reducers
    ),
    EffectsModule.forFeature([MaintenanceTeamsEffects]),
    // TODO after creation of CRUD Team MaintenanceTeam : select the optioDto dommain module required for link
    // Domain Modules:
    // BIAToolKit - Begin Option Airport
    AirportOptionModule,
    // BIAToolKit - End Option Airport
    // BIAToolKit - Begin Option Country
    CountryOptionModule,
    // BIAToolKit - End Option Country
  ],
})
export class MaintenanceTeamModule {}
