import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { AirportsEffects } from './store/airports-effects';
import { reducers } from './store/airport.state';
import { AirportFormComponent } from './components/airport-form/airport-form.component';
import { AirportsIndexComponent } from './views/airports-index/airports-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { AirportNewComponent } from './views/airport-new/airport-new.component';
import { AirportEditComponent } from './views/airport-edit/airport-edit.component';
import { Permission } from 'src/app/shared/permission';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { AirportItemComponent } from './views/airport-item/airport-item.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { AirportTableComponent } from './components/airport-table/airport-table.component';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Airport_List_Access,
      InjectComponent: AirportsIndexComponent
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
          InjectComponent: AirportNewComponent,
        },
        component: PopupLayoutComponent,
        // component: FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':airportId',
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
              InjectComponent: AirportEditComponent,
            },
            component: PopupLayoutComponent,
            // component: FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
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
    AirportItemComponent,
    // [Calc] : NOT used only for calc (4 lines).
    // it is possible to delete unsed commponent files (views/..-new + views/..-edit + components/...-form).
    AirportFormComponent,
    AirportsIndexComponent,
    AirportNewComponent,
    AirportEditComponent,
    // [Calc] : Used only for calc it is possible to delete unsed commponent files (components/...-table)).
    AirportTableComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('airports', reducers),
    EffectsModule.forFeature([AirportsEffects]),
    // Domain Modules:
  ]
})
export class AirportModule {
}

