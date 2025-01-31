import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { ReducerManager, StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { Permission } from 'src/app/shared/permission';
import { SharedModule } from 'src/app/shared/shared.module';
import { PlaneFormComponent } from './components/plane-form/plane-form.component';
import { PlaneItemComponent } from './views/plane-item/plane-item.component';
import { PlanesIndexComponent } from './views/planes-index/planes-index.component';
// BIAToolKit - Begin Option Airport
import { AirportOptionModule } from 'src/app/domains/airport-option/airport-option.module';
// BIAToolKit - End Option Airport
// BIAToolKit - Begin Option PlaneType
import { PlaneTypeOptionModule } from 'src/app/domains/plane-type-option/plane-type-option.module';
// BIAToolKit - End Option PlaneType
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { CrudItemImportModule } from 'src/app/shared/bia-shared/feature-templates/crud-items/crud-item-import.module';
import { CrudItemModule } from 'src/app/shared/bia-shared/feature-templates/crud-items/crud-item.module';
import { PlaneTableComponent } from './components/plane-table/plane-table.component';
import { planeCRUDConfiguration } from './plane.constants';
import { FeaturePlanesStore } from './store/plane.state';
import { PlanesEffects } from './store/planes-effects';
import { PlaneEditComponent } from './views/plane-edit/plane-edit.component';
import { PlaneImportComponent } from './views/plane-import/plane-import.component';
import { PlaneNewComponent } from './views/plane-new/plane-new.component';

export const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Plane_List_Access,
      injectComponent: PlanesIndexComponent,
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
          permission: Permission.Plane_Create,
          title: 'plane.add',
          injectComponent: PlaneNewComponent,
          dynamicComponent: () =>
            planeCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
        },
        component: planeCRUDConfiguration.usePopup
          ? PopupLayoutComponent
          : FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: 'import',
        data: {
          breadcrumb: 'plane.import',
          canNavigate: false,
          style: {
            minWidth: '80vw',
            maxWidth: '80vw',
            maxHeight: '80vh',
          },
          permission: Permission.Plane_Save,
          title: 'plane.import',
          injectComponent: PlaneImportComponent,
          dynamicComponent: () =>
            planeCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
        },
        component: planeCRUDConfiguration.usePopup
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
        component: PlaneItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.Plane_Update,
              title: 'plane.edit',
              injectComponent: PlaneEditComponent,
              dynamicComponent: () =>
                planeCRUDConfiguration.usePopup
                  ? PopupLayoutComponent
                  : FullPageLayoutComponent,
            },
            component: planeCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
          /// BIAToolKit - Begin Partial PlaneModuleChildPath Engine
          {
            path: 'engines',
            data: {
              breadcrumb: 'app.engines',
              canNavigate: true,
              permission: Permission.Engine_List_Access,
            },
            loadChildren: () =>
              import('./children/engines/engine.module').then(
                m => m.EngineModule
              ),
          },
          /// BIAToolKit - End Partial PlaneModuleChildPath Engine
          // BIAToolKit - Begin PlaneModuleChildPath
          // BIAToolKit - End PlaneModuleChildPath
        ],
      },
    ],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  declarations: [
    PlaneItemComponent,
    PlanesIndexComponent,
    // [Calc] : NOT used for calc (3 lines).
    // it is possible to delete unsed commponent files (views/..-new + views/..-edit + components/...-form).
    PlaneFormComponent,
    PlaneNewComponent,
    PlaneEditComponent,
    // [Calc] : Used only for calc it is possible to delete unsed commponent files (components/...-table)).
    PlaneTableComponent,
    PlaneImportComponent,
  ],
  imports: [
    SharedModule,
    CrudItemModule,
    CrudItemImportModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature(
      planeCRUDConfiguration.storeKey,
      FeaturePlanesStore.reducers
    ),
    EffectsModule.forFeature([PlanesEffects]),
    // TODO after creation of CRUD Plane : select the optioDto dommain module required for link
    // Domain Modules:
    // BIAToolKit - Begin Option Airport
    AirportOptionModule,
    // BIAToolKit - End Option Airport
    // BIAToolKit - Begin Option PlaneType
    PlaneTypeOptionModule,
    // BIAToolKit - End Option PlaneType
  ],
})
export class PlaneModule {}
