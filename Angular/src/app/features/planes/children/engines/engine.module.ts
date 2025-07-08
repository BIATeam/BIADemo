import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { PartOptionModule } from 'src/app/domains/part-option/part-option.module';
import {
  DynamicLayoutComponent,
  LayoutMode,
} from 'src/app/shared/bia-shared/components/layout/dynamic-layout/dynamic-layout.component';
import { Permission } from 'src/app/shared/permission';
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
      configuration: engineCRUDConfiguration,
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
          permission: Permission.Engine_Create,
          title: 'engine.add',
        },
        component: EngineNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: 'import',
        data: {
          breadcrumb: 'engine.import',
          canNavigate: false,
          layoutMode: LayoutMode.popup,
          style: {
            minWidth: '80vw',
            maxWidth: '80vw',
            maxHeight: '80vh',
          },
          permission: Permission.Engine_Save,
          title: 'engine.import',
        },
        component: EngineImportComponent,
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
            },
            component: EngineEditComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
          // BIAToolKit - Begin EngineModuleChildPath
          // BIAToolKit - End EngineModuleChildPath
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
      engineCRUDConfiguration.storeKey,
      FeatureEnginesStore.reducers
    ),
    EffectsModule.forFeature([EnginesEffects]),
    // Domain Modules:
    PartOptionModule,
  ],
})
export class EngineModule {}
