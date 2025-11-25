import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import {
  BiaPermission,
  PermissionGuard,
} from 'packages/bia-ng/core/public-api';
import { ViewSaveComponent } from 'packages/bia-ng/shared/features/view/views/view-save/view-save.component';
import {
  LayoutMode,
  viewCRUDConfiguration,
  ViewsEffects,
  ViewsStore,
} from 'packages/bia-ng/shared/public-api';
import { ViewItemComponent } from './views/view-item/view-item.component';

export const ROUTES: Routes = [
  {
    path: ':crudItemId',
    data: {
      breadcrumb: '',
      canNavigate: false,
    },
    component: ViewItemComponent,
    children: [
      {
        path: 'saveView',
        data: {
          breadcrumb: 'bia.views.saveTitle',
          canNavigate: true,
          permission: BiaPermission.View_List,
          layoutMode: LayoutMode.splitPage,
          title: 'bia.views.saveTitle',
        },
        component: ViewSaveComponent,
        canActivate: [PermissionGuard],
      },
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature(viewCRUDConfiguration.storeKey, ViewsStore.reducers),
    EffectsModule.forFeature([ViewsEffects]),
    // Domain Modules:
  ],
})
export class BiaViewModule {}
