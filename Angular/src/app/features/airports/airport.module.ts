import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { ReducerManager, StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { DynamicLayoutComponent } from 'src/app/shared/bia-shared/components/layout/dynamic-layout/dynamic-layout.component';
import { Permission } from 'src/app/shared/permission';

import { airportCRUDConfiguration } from './airport.constants';
import { FeatureAirportsStore } from './store/airport.state';
import { AirportsEffects } from './store/airports-effects';
import { AirportEditComponent } from './views/airport-edit/airport-edit.component';
import { AirportItemComponent } from './views/airport-item/airport-item.component';
import { AirportNewComponent } from './views/airport-new/airport-new.component';
import { AirportsIndexComponent } from './views/airports-index/airports-index.component';

export const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Airport_List_Access,
      injectComponent: AirportsIndexComponent,
      configuration: airportCRUDConfiguration,
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
          permission: Permission.Airport_Create,
          title: 'airport.add',
        },
        component: AirportNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: true,
        },
        component: AirportItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.Airport_Update,
              title: 'airport.edit',
            },
            component: AirportEditComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
          // BIAToolKit - Begin AirportModuleChildPath
          // BIAToolKit - End AirportModuleChildPath
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
      airportCRUDConfiguration.storeKey,
      FeatureAirportsStore.reducers
    ),
    EffectsModule.forFeature([AirportsEffects]),
  ],
})
export class AirportModule {}
