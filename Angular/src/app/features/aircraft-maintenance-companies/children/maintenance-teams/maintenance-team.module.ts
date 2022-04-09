import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { MaintenanceTeamsEffects } from './store/maintenance-teams-effects';
import { reducers } from './store/maintenance-team.state';
import { MaintenanceTeamFormComponent } from './components/maintenance-team-form/maintenance-team-form.component';
import { MaintenanceTeamsIndexComponent } from './views/maintenance-teams-index/maintenance-teams-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { MaintenanceTeamNewComponent } from './views/maintenance-team-new/maintenance-team-new.component';
import { MaintenanceTeamEditComponent } from './views/maintenance-team-edit/maintenance-team-edit.component';
import { Permission } from 'src/app/shared/permission';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { MaintenanceTeamItemComponent } from './views/maintenance-team-item/maintenance-team-item.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { MaintenanceTeamTableComponent } from './components/maintenance-team-table/maintenance-team-table.component';
import { storeKey, usePopup } from './maintenance-team.constants';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.MaintenanceTeam_List_Access,
      InjectComponent: MaintenanceTeamsIndexComponent
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
          InjectComponent: MaintenanceTeamNewComponent,
        },
        component: (usePopup) ? PopupLayoutComponent : FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':maintenanceTeamId',
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
              permission: Permission.MaintenanceTeam_Member_List_Access
            },
            loadChildren: () =>
              import('./children/members/maintenance-team-member.module').then((m) => m.MaintenanceTeamMemberModule)
          },
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.MaintenanceTeam_Update,
              title: 'maintenanceTeam.edit',
              InjectComponent: MaintenanceTeamEditComponent,
            },
            component: (usePopup) ? PopupLayoutComponent : FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
        ]
      },
    ]
  },
  { path: '**', redirectTo: '' }
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
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature(storeKey, reducers),
    EffectsModule.forFeature([MaintenanceTeamsEffects]),
    // Domain Modules:
  ]
})

export class MaintenanceTeamModule {
}

