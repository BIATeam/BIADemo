import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { storeKey } from './country-option.constants';
import { reducers } from './store/country-option.state';
import { CountryOptionsEffects } from './store/country-options-effects';

@NgModule({
  imports: [
    StoreModule.forFeature(storeKey, reducers),
    EffectsModule.forFeature([CountryOptionsEffects]),
  ],
})
export class CountryOptionModule {}
