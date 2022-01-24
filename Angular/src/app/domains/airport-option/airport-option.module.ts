import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { storeKey } from './airport-option.contants';
import { reducers } from './store/airport-option.state';
import { AirportOptionsEffects } from './store/airport-options-effects';


@NgModule({
  imports: [
    StoreModule.forFeature(storeKey, reducers),
    EffectsModule.forFeature([AirportOptionsEffects]),
  ]
})
export class AirportOptionModule {}


















