import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { CoreTeamsStore } from './store/team.state';
import { TeamsEffects } from './store/teams-effects';
import { storeKey } from './team.constants';

@NgModule({
  imports: [
    StoreModule.forFeature(storeKey, CoreTeamsStore.reducers),
    EffectsModule.forFeature([TeamsEffects]),
  ],
})
export class TeamModule {}
