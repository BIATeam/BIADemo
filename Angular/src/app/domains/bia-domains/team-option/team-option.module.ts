import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/team-option.state';
import { TeamOptionsEffects } from './store/team-options-effects';
import { storeKey } from './team-option.contants';

@NgModule({
  imports: [
    StoreModule.forFeature(storeKey, reducers),
    EffectsModule.forFeature([TeamOptionsEffects]),
  ],
})
export class TeamOptionModule {}
