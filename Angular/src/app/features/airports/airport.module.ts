import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { AirportsEffects } from './store/airports-effects';
import { reducers } from './store/airport.state';
import { AirportFormComponent } from './components/airport-form/airport-form.component';
import { AirportsIndexComponent } from './views/airports-index/airports-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { AirportEditDialogComponent } from './views/airport-edit-dialog/airport-edit-dialog.component';
import { AirportNewDialogComponent } from './views/airport-new-dialog/airport-new-dialog.component';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { Permission } from 'src/app/shared/permission';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Airport_List_Access
    },
    component: AirportsIndexComponent,
    canActivate: [PermissionGuard]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [
    AirportFormComponent,
    AirportsIndexComponent,
    AirportNewDialogComponent,
    AirportEditDialogComponent,
  ],
  entryComponents: [AirportNewDialogComponent, AirportEditDialogComponent],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('airports', reducers),
    EffectsModule.forFeature([AirportsEffects])
  ]
})
export class AirportModule {}
