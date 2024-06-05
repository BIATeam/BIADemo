import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { ReducerManager, StoreModule } from '@ngrx/store';
import { PlaneTypeFormComponent } from './components/plane-type-form/plane-type-form.component';
import { PlanesTypesIndexComponent } from './views/planes-types-index/planes-types-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { Permission } from 'src/app/shared/permission';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { PlaneTypeItemComponent } from './views/plane-type-item/plane-type-item.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { PlaneTypeTableComponent } from './components/plane-type-table/plane-type-table.component';
import { CrudItemModule } from 'src/app/shared/bia-shared/feature-templates/crud-items/crud-item.module';
import { PlaneTypeEditComponent } from './views/plane-type-edit/plane-type-edit.component';
import { PlaneTypeNewComponent } from './views/plane-type-new/plane-type-new.component';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { PlanesTypesEffects } from './store/planes-types-effects';
import { FeaturePlanesTypesStore } from './store/plane-type.state';
import { PlaneTypeCRUDConfiguration } from './plane-type.constants';

export const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.PlaneType_List_Access,
      InjectComponent: PlanesTypesIndexComponent,
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
          permission: Permission.PlaneType_Create,
          title: 'planeType.add',
          InjectComponent: PlaneTypeNewComponent,
          dynamicComponent: () =>
            PlaneTypeCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
        },
        component: PlaneTypeCRUDConfiguration.usePopup
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
              InjectComponent: PlaneTypeEditComponent,
              dynamicComponent: () =>
                PlaneTypeCRUDConfiguration.usePopup
                  ? PopupLayoutComponent
                  : FullPageLayoutComponent,
            },
            component: PlaneTypeCRUDConfiguration.usePopup
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
    PlaneTypeItemComponent,
    PlanesTypesIndexComponent,
    // [Calc] : NOT used for calc (3 lines).
    // it is possible to delete unsed commponent files (views/..-new + views/..-edit + components/...-form).
    PlaneTypeFormComponent,
    PlaneTypeNewComponent,
    PlaneTypeEditComponent,
    // [Calc] : Used only for calc it is possible to delete unsed commponent files (components/...-table)).
    PlaneTypeTableComponent,
  ],
  imports: [
    SharedModule,
    CrudItemModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature(
      PlaneTypeCRUDConfiguration.storeKey,
      FeaturePlanesTypesStore.reducers
    ),
    EffectsModule.forFeature([PlanesTypesEffects]),
    // TODO after creation of CRUD PlaneType : select the optioDto dommain module requiered for link
    // Domain Modules:
  ],
})
export class PlaneTypeModule {}
