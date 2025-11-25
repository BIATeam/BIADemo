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
import { PilotReadComponent } from '../pilots/views/pilot-read/pilot-read.component';
import { pilotCRUDConfiguration } from './pilot.constants';
import { PilotService } from './services/pilot.service';
import { FeaturePilotsStore } from './store/pilot.state';
import { PilotsEffects } from './store/pilots-effects';
import { PilotEditComponent } from './views/pilot-edit/pilot-edit.component';
import { PilotImportComponent } from './views/pilot-import/pilot-import.component';
import { PilotItemComponent } from './views/pilot-item/pilot-item.component';
import { PilotNewComponent } from './views/pilot-new/pilot-new.component';
import { PilotsIndexComponent } from './views/pilots-index/pilots-index.component';

export const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Pilot_List_Access,
      injectComponent: PilotsIndexComponent,
      configuration: pilotCRUDConfiguration,
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
          permission: Permission.Pilot_Create,
          title: 'pilot.add',
        },
        component: PilotNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: 'view',
        data: {
          featureViews: pilotCRUDConfiguration.featureName,
          featureConfiguration: pilotCRUDConfiguration,
          featureServiceType: PilotService,
          leftWidth: 60,
        },
        loadChildren: () =>
          import('../../shared/bia-shared/view.module').then(m => m.ViewModule),
      },
      {
        path: 'import',
        data: {
          breadcrumb: 'pilot.import',
          canNavigate: false,
          layoutMode: LayoutMode.popup,
          style: {
            minWidth: '80vw',
            maxWidth: '80vw',
            maxHeight: '80vh',
          },
          permission: Permission.Pilot_Save,
          title: 'pilot.import',
        },
        component: PilotImportComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: false,
        },
        component: PilotItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'read',
            data: {
              breadcrumb: 'bia.read',
              canNavigate: true,
              permission: Permission.Pilot_Read,
              readOnlyMode: pilotCRUDConfiguration.formEditReadOnlyMode,
              title: 'pilot.read',
            },
            component: PilotReadComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.Pilot_Update,
              title: 'pilot.edit',
            },
            component: PilotEditComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'read',
          },
          // BIAToolKit - Begin PilotModuleChildPath
          // BIAToolKit - End PilotModuleChildPath
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
      pilotCRUDConfiguration.storeKey,
      FeaturePilotsStore.reducers
    ),
    EffectsModule.forFeature([PilotsEffects]),
    // Domain Modules:
  ],
})
export class PilotModule {}
