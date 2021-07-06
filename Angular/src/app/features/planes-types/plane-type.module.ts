import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { PlanesTypesEffects } from './store/planes-types-effects';
import { reducers } from './store/plane-type.state';
import { PlaneTypeFormComponent } from './components/plane-type-form/plane-type-form.component';
import { PlanesTypesIndexComponent } from './views/planes-types-index/planes-types-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PlaneTypeEditDialogComponent } from './views/plane-type-edit-dialog/plane-type-edit-dialog.component';
import { PlaneTypeNewDialogComponent } from './views/plane-type-new-dialog/plane-type-new-dialog.component';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { Permission } from 'src/app/shared/permission';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.PlaneType_List_Access
    },
    component: PlanesTypesIndexComponent,
    canActivate: [PermissionGuard]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [
    PlaneTypeFormComponent,
    PlanesTypesIndexComponent,
    PlaneTypeNewDialogComponent,
    PlaneTypeEditDialogComponent,
  ],
  entryComponents: [PlaneTypeNewDialogComponent, PlaneTypeEditDialogComponent],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('planes-types', reducers),
    EffectsModule.forFeature([PlanesTypesEffects])
  ]
})
export class PlaneTypeModule {}
