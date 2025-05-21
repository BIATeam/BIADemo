import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { AirportOptionModule } from 'src/app/domains/airport-option/airport-option.module';
import { PlaneTypeOptionModule } from 'src/app/domains/plane-type-option/plane-type-option.module';
import {
  DynamicLayoutComponent,
  LayoutMode,
} from 'src/app/shared/bia-shared/components/layout/dynamic-layout/dynamic-layout.component';
import { Permission } from 'src/app/shared/permission';
import { PlaneReadComponent } from '../planes/views/plane-read/plane-read.component';
import { planeCRUDConfiguration } from './plane.constants';
import { FeaturePlanesStore } from './store/plane.state';
import { PlanesEffects } from './store/planes-effects';
import { PlaneEditComponent } from './views/plane-edit/plane-edit.component';
import { PlaneImportComponent } from './views/plane-import/plane-import.component';
import { PlaneItemComponent } from './views/plane-item/plane-item.component';
import { PlaneNewComponent } from './views/plane-new/plane-new.component';
import { PlanesIndexComponent } from './views/planes-index/planes-index.component';

export const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Plane_List_Access,
      injectComponent: PlanesIndexComponent,
      configuration: planeCRUDConfiguration,
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
          permission: Permission.Plane_Create,
          title: 'plane.add',
        },
        component: PlaneNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: 'import',
        data: {
          breadcrumb: 'plane.import',
          canNavigate: false,
          layoutMode: LayoutMode.popup,
          style: {
            minWidth: '80vw',
            maxWidth: '80vw',
            maxHeight: '80vh',
          },
          permission: Permission.Plane_Save,
          title: 'plane.import',
        },
        component: PlaneImportComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: true,
        },
        component: PlaneItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'read',
            data: {
              breadcrumb: 'bia.read',
              canNavigate: true,
              permission: Permission.Plane_Read,
              readOnlyMode: planeCRUDConfiguration.formEditReadOnlyMode,
              title: 'plane.read',
            },
            component: PlaneReadComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.Plane_Update,
              title: 'plane.edit',
            },
            component: PlaneEditComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'read',
          },
          // Begin BIADemo
          // BIAToolKit - Begin Partial PlaneModuleChildPath Engine
          {
            path: 'engines',
            data: {
              breadcrumb: 'app.engines',
              canNavigate: true,
              permission: Permission.Engine_List_Access,
              layoutMode: LayoutMode.fullPage,
            },
            loadChildren: () =>
              import('./children/engines/engine.module').then(
                m => m.EngineModule
              ),
          },
          // BIAToolKit - End Partial PlaneModuleChildPath Engine
          // End BIADemo
          // BIAToolKit - Begin PlaneModuleChildPath
          // BIAToolKit - End PlaneModuleChildPath
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
      planeCRUDConfiguration.storeKey,
      FeaturePlanesStore.reducers
    ),
    EffectsModule.forFeature([PlanesEffects]),
    // TODO after creation of CRUD Plane : select the optionDto domain module required for link
    // Domain Modules:
    PlaneTypeOptionModule,
    AirportOptionModule,
  ],
})
export class PlaneModule {}
