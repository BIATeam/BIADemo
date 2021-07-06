import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/airport-option.state';
import { AirportOptionsEffects } from './store/airport-options-effects';


@NgModule({
  imports: [
    StoreModule.forFeature('domain-airports', reducers),
    EffectsModule.forFeature([AirportOptionsEffects]),
  ]
})
export class AirportOptionModule {}


















