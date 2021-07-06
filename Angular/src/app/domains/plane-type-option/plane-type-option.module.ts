import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/plane-type-option.state';
import { PlaneTypeOptionsEffects } from './store/plane-type-options-effects';


@NgModule({
  imports: [
    StoreModule.forFeature('domain-planes-types', reducers),
    EffectsModule.forFeature([PlaneTypeOptionsEffects]),
  ]
})
export class PlaneTypeOptionModule {}


















