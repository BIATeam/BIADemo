import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { ReducerManager, StoreModule } from '@ngrx/store';
import { EngineFormComponent } from './components/engine-form/engine-form.component';
import { EnginesIndexComponent } from './views/engines-index/engines-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { Permission } from 'src/app/shared/permission';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { EngineItemComponent } from './views/engine-item/engine-item.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { EngineTableComponent } from './components/engine-table/engine-table.component';
import { CrudItemModule } from 'src/app/shared/bia-shared/feature-templates/crud-items/crud-item.module';
import { EngineEditComponent } from './views/engine-edit/engine-edit.component';
import { EngineNewComponent } from './views/engine-new/engine-new.component';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { EnginesEffects } from './store/engines-effects';
import { FeatureEnginesStore } from './store/engine.state';
import { EngineCRUDConfiguration } from './engine.constants';

export let ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Engine_List_Access,
      InjectComponent: EnginesIndexComponent
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
          InjectComponent: EngineNewComponent,
          dynamicComponent : () => (EngineCRUDConfiguration.usePopup) ? PopupLayoutComponent : FullPageLayoutComponent,
        },
        component: (EngineCRUDConfiguration.usePopup) ? PopupLayoutComponent : FullPageLayoutComponent,
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
              InjectComponent: EngineEditComponent,
              dynamicComponent : () => (EngineCRUDConfiguration.usePopup) ? PopupLayoutComponent : FullPageLayoutComponent,
            },
            component: (EngineCRUDConfiguration.usePopup) ? PopupLayoutComponent : FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit'
          },
        ]
      },
    ]
  },
  { path: '**', redirectTo: '' }
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
  ],
  imports: [
    SharedModule,
    CrudItemModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature(EngineCRUDConfiguration.storeKey, FeatureEnginesStore.reducers),
    EffectsModule.forFeature([EnginesEffects]),
    // TODO after creation of CRUD Engine : select the optioDto dommain module requiered for link
    // Domain Modules:
  ]
})

export class EngineModule {
}

