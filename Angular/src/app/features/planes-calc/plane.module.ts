import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { PlanesEffects } from './store/planes-effects';
import { reducers } from './store/plane.state';
import { PlanesIndexComponent } from './views/planes-index/planes-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { Permission } from 'src/app/shared/permission';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { PlaneTableComponent } from './components/plane-table/plane-table.component';
import { AirportOptionModule } from 'src/app/domains/airport-option/airport-option.module';
import { PlaneTypeOptionModule } from 'src/app/domains/plane-type-option/plane-type-option.module';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Plane_List_Access
    },
    component: PlanesIndexComponent,
    canActivate: [PermissionGuard]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [PlanesIndexComponent, PlaneTableComponent],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('planes-mode-calc', reducers),
    EffectsModule.forFeature([PlanesEffects]),
    // Domain Modules:
    AirportOptionModule,
    PlaneTypeOptionModule
  ]
})
export class PlaneModule { }
