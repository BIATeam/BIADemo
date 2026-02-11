import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'packages/bia-ng/core/public-api';
import { DynamicLayoutComponent } from 'packages/bia-ng/shared/public-api';
import { Permission } from 'src/app/shared/permission';
import { planeTypeCRUDConfiguration } from './plane-type.constants';
import { PlaneTypeService } from './services/plane-type.service';
import { FeaturePlanesTypesStore } from './store/plane-type.state';
import { PlanesTypesEffects } from './store/planes-types-effects';
import { PlaneTypeEditComponent } from './views/plane-type-edit/plane-type-edit.component';
import { PlaneTypeItemComponent } from './views/plane-type-item/plane-type-item.component';
import { PlaneTypeNewComponent } from './views/plane-type-new/plane-type-new.component';
import { PlanesTypesIndexComponent } from './views/planes-types-index/planes-types-index.component';

export const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.PlaneType_List_Access,
      injectComponent: PlanesTypesIndexComponent,
      configuration: planeTypeCRUDConfiguration,
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
          permission: Permission.PlaneType_Create,
          title: 'planeType.add',
        },
        component: PlaneTypeNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: 'view',
        data: {
          featureConfiguration: planeTypeCRUDConfiguration,
          featureServiceType: PlaneTypeService,
          leftWidth: 60,
        },
        loadChildren: () =>
          import('../../shared/bia-shared/view.module').then(m => m.ViewModule),
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: false,
        },
        component: PlaneTypeItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.PlaneType_Update,
              title: 'planeType.edit',
            },
            component: PlaneTypeEditComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
          // BIAToolKit - Begin PlaneTypeModuleChildPath
          // BIAToolKit - End PlaneTypeModuleChildPath
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
      planeTypeCRUDConfiguration.storeKey,
      FeaturePlanesTypesStore.reducers
    ),
    EffectsModule.forFeature([PlanesTypesEffects]),
  ],
})
export class PlaneTypeModule {}
