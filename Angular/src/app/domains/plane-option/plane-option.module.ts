import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { storeKey } from './plane-option.contants';
import { reducers } from './store/plane-option.state';
import { PlaneOptionsEffects } from './store/plane-options-effects';

@NgModule({
  imports: [
    StoreModule.forFeature(storeKey, reducers),
    EffectsModule.forFeature([PlaneOptionsEffects]),
  ],
})
export class PlaneOptionModule {}
