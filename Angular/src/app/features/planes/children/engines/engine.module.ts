import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { ReducerManager, StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { PartOptionModule } from 'src/app/domains/part-option/part-option.module';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { CrudItemImportModule } from 'src/app/shared/bia-shared/feature-templates/crud-items/crud-item-import.module';
import { CrudItemModule } from 'src/app/shared/bia-shared/feature-templates/crud-items/crud-item.module';
import { Permission } from 'src/app/shared/permission';
import { SharedModule } from 'src/app/shared/shared.module';
import { EngineFormComponent } from './components/engine-form/engine-form.component';
import { EngineTableComponent } from './components/engine-table/engine-table.component';
import { engineCRUDConfiguration } from './engine.constants';
import { FeatureEnginesStore } from './store/engine.state';
import { EnginesEffects } from './store/engines-effects';
import { EngineEditComponent } from './views/engine-edit/engine-edit.component';
import { EngineImportComponent } from './views/engine-import/engine-import.component';
import { EngineItemComponent } from './views/engine-item/engine-item.component';
import { EngineNewComponent } from './views/engine-new/engine-new.component';
import { EnginesIndexComponent } from './views/engines-index/engines-index.component';

export const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Engine_List_Access,
      injectComponent: EnginesIndexComponent,
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
          permission: Permission.Engine_Create,
          title: 'engine.add',
          injectComponent: EngineNewComponent,
          dynamicComponent: () =>
            engineCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
        },
        component: engineCRUDConfiguration.usePopup
          ? PopupLayoutComponent
          : FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: 'import',
        data: {
          breadcrumb: 'engine.import',
          canNavigate: false,
          style: {
            minWidth: '80vw',
            maxWidth: '80vw',
            maxHeight: '80vh',
          },
          permission: Permission.Engine_Save,
          title: 'engine.import',
          injectComponent: EngineImportComponent,
          dynamicComponent: () =>
            engineCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
        },
        component: engineCRUDConfiguration.usePopup
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
        component: EngineItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.Engine_Update,
              title: 'engine.edit',
              injectComponent: EngineEditComponent,
              dynamicComponent: () =>
                engineCRUDConfiguration.usePopup
                  ? PopupLayoutComponent
                  : FullPageLayoutComponent,
            },
            component: engineCRUDConfiguration.usePopup
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
    EngineItemComponent,
    EnginesIndexComponent,
    // [Calc] : NOT used for calc (3 lines).
    // it is possible to delete unsed commponent files (views/..-new + views/..-edit + components/...-form).
    EngineFormComponent,
    EngineNewComponent,
    EngineEditComponent,
    // [Calc] : Used only for calc it is possible to delete unsed commponent files (components/...-table)).
    EngineTableComponent,
    EngineImportComponent,
  ],
  imports: [
    SharedModule,
    CrudItemModule,
    CrudItemImportModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature(
      engineCRUDConfiguration.storeKey,
      FeatureEnginesStore.reducers
    ),
    EffectsModule.forFeature([EnginesEffects]),
    // TODO after creation of CRUD Engine : select the optioDto dommain module required for link
    // Domain Modules:
    PartOptionModule,
  ],
})
export class EngineModule {}
