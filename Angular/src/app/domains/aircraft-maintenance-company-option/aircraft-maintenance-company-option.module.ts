import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { storeKey } from './aircraft-maintenance-company-option.contants';
import { reducers } from './store/aircraft-maintenance-company-option.state';
import { AircraftMaintenanceCompanyOptionsEffects } from './store/aircraft-maintenance-company-options-effects';

@NgModule({
  imports: [
    StoreModule.forFeature(storeKey, reducers),
    EffectsModule.forFeature([AircraftMaintenanceCompanyOptionsEffects]),
  ],
})
export class AircraftMaintenanceCompanyOptionModule {}
