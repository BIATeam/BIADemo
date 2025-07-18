import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { DomainTeamOptionsStore } from './store/team-option.state';
import { TeamOptionsEffects } from './store/team-options-effects';
import { storeKey } from './team-option.contants';

@NgModule({
  imports: [
    StoreModule.forFeature(storeKey, DomainTeamOptionsStore.reducers),
    EffectsModule.forFeature([TeamOptionsEffects]),
  ],
})
export class TeamOptionModule {}
