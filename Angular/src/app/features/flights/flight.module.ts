import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PermissionGuard } from '@bia-team/bia-ng/core';
import { DynamicLayoutComponent, LayoutMode } from '@bia-team/bia-ng/shared';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { AirportOptionModule } from 'src/app/domains/airport-option/airport-option.module';
import { Permission } from 'src/app/shared/permission';
import { FlightReadComponent } from '../flights/views/flight-read/flight-read.component';
import { flightCRUDConfiguration } from './flight.constants';
import { FlightService } from './services/flight.service';
import { FeatureFlightsStore } from './store/flight.state';
import { FlightsEffects } from './store/flights-effects';
import { FlightEditComponent } from './views/flight-edit/flight-edit.component';
import { FlightImportComponent } from './views/flight-import/flight-import.component';
import { FlightItemComponent } from './views/flight-item/flight-item.component';
import { FlightNewComponent } from './views/flight-new/flight-new.component';
import { FlightsIndexComponent } from './views/flights-index/flights-index.component';

export const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Flight_List_Access,
      injectComponent: FlightsIndexComponent,
      configuration: flightCRUDConfiguration,
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
          permission: Permission.Flight_Create,
          title: 'flight.add',
        },
        component: FlightNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: 'view',
        data: {
          featureConfiguration: flightCRUDConfiguration,
          featureServiceType: FlightService,
          leftWidth: 60,
        },
        loadChildren: () =>
          import('../../shared/bia-shared/view.module').then(m => m.ViewModule),
      },
      {
        path: 'import',
        data: {
          breadcrumb: 'flight.import',
          canNavigate: false,
          layoutMode: LayoutMode.popup,
          style: {
            minWidth: '80vw',
            maxWidth: '80vw',
            maxHeight: '80vh',
          },
          permission: Permission.Flight_Save,
          title: 'flight.import',
        },
        component: FlightImportComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: false,
        },
        component: FlightItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'read',
            data: {
              breadcrumb: 'bia.read',
              canNavigate: true,
              permission: Permission.Flight_Read,
              readOnlyMode: flightCRUDConfiguration.formEditReadOnlyMode,
              title: 'flight.read',
            },
            component: FlightReadComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.Flight_Update,
              title: 'flight.edit',
            },
            component: FlightEditComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'read',
          },
          // BIAToolKit - Begin FlightModuleChildPath
          // BIAToolKit - End FlightModuleChildPath
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
      flightCRUDConfiguration.storeKey,
      FeatureFlightsStore.reducers
    ),
    EffectsModule.forFeature([FlightsEffects]),
    // Domain Modules:
    AirportOptionModule,
  ],
})
export class FlightModule {}
