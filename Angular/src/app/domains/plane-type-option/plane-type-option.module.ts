import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { storeKey } from './plane-type-option.contants';
import { reducers } from './store/plane-type-option.state';
import { PlaneTypeOptionsEffects } from './store/plane-type-options-effects';

@NgModule({
  imports: [
    StoreModule.forFeature(storeKey, reducers),
    EffectsModule.forFeature([PlaneTypeOptionsEffects]),
  ],
})
export class PlaneTypeOptionModule {}
