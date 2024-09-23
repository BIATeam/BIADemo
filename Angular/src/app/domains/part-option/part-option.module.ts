import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { storeKey } from './part-option.contants';
import { reducers } from './store/part-option.state';
import { PartOptionsEffects } from './store/part-options-effects';

@NgModule({
  imports: [
    StoreModule.forFeature(storeKey, reducers),
    EffectsModule.forFeature([PartOptionsEffects]),
  ],
})
export class PartOptionModule {}
