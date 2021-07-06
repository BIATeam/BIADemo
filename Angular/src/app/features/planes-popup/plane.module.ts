import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { PlanesEffects } from './store/planes-effects';
import { reducers } from './store/plane.state';
import { PlaneFormComponent } from './components/plane-form/plane-form.component';
import { PlanesIndexComponent } from './views/planes-index/planes-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PlaneEditDialogComponent } from './views/plane-edit-dialog/plane-edit-dialog.component';
import { PlaneNewDialogComponent } from './views/plane-new-dialog/plane-new-dialog.component';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { Permission } from 'src/app/shared/permission';
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
  declarations: [
    PlaneFormComponent,
    PlanesIndexComponent,
    PlaneNewDialogComponent,
    PlaneEditDialogComponent,
  ],
  entryComponents: [PlaneNewDialogComponent, PlaneEditDialogComponent],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('planes', reducers),
    EffectsModule.forFeature([PlanesEffects]),
    // Domain Modules:
    AirportOptionModule,
    PlaneTypeOptionModule,
  ]
})
export class PlaneModule {}
