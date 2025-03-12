import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { ReducerManager, StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { CrudItemModule } from 'src/app/shared/bia-shared/feature-templates/crud-items/crud-item.module';
import { Permission } from 'src/app/shared/permission';
import { SharedModule } from 'src/app/shared/shared.module';
import { airportCRUDConfiguration } from './airport.constants';
import { AirportFormComponent } from './components/airport-form/airport-form.component';
import { AirportTableComponent } from './components/airport-table/airport-table.component';
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
          permission: Permission.Airport_Create,
          title: 'airport.add',
          injectComponent: AirportNewComponent,
          dynamicComponent: () =>
            airportCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
        },
        component: airportCRUDConfiguration.usePopup
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
              injectComponent: AirportEditComponent,
              dynamicComponent: () =>
                airportCRUDConfiguration.usePopup
                  ? PopupLayoutComponent
                  : FullPageLayoutComponent,
            },
            component: airportCRUDConfiguration.usePopup
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
        SharedModule,
        CrudItemModule,
        RouterModule.forChild(ROUTES),
        StoreModule.forFeature(airportCRUDConfiguration.storeKey, FeatureAirportsStore.reducers),
        EffectsModule.forFeature([AirportsEffects]),
        AirportItemComponent,
        AirportsIndexComponent,
        // [Calc] : NOT used for calc (3 lines).
        // it is possible to delete unsed commponent files (views/..-new + views/..-edit + components/...-form).
        AirportFormComponent,
        AirportNewComponent,
        AirportEditComponent,
        // [Calc] : Used only for calc it is possible to delete unsed commponent files (components/...-table)).
        AirportTableComponent,
    ],
})
export class AirportModule {}
