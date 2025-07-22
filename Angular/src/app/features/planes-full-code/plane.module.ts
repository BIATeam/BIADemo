﻿import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'biang/core';
import { FullPageLayoutComponent, PopupLayoutComponent } from 'biang/shared';
import { AirportOptionModule } from 'src/app/domains/airport-option/airport-option.module';
import { PlaneTypeOptionModule } from 'src/app/domains/plane-type-option/plane-type-option.module';
import { Permission } from 'src/app/shared/permission';

import { storeKey, usePopup } from './plane.constants';
import { reducers } from './store/plane.state';
import { PlanesEffects } from './store/planes-effects';
import { PlaneEditComponent } from './views/plane-edit/plane-edit.component';
import { PlaneItemComponent } from './views/plane-item/plane-item.component';
import { PlaneNewComponent } from './views/plane-new/plane-new.component';
import { PlanesIndexComponent } from './views/planes-index/planes-index.component';

const ROUTES: Routes = [
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
        },
        component: usePopup ? PopupLayoutComponent : FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':planeId',
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
            },
            component: usePopup
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
  imports: [
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature(storeKey, reducers),
    EffectsModule.forFeature([PlanesEffects]),
    // Domain Modules:
    AirportOptionModule,
    PlaneTypeOptionModule,
  ],
})
export class PlaneModule {}
