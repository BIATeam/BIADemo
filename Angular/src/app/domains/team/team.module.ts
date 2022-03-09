import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { storeKey } from './team.contants';
import { reducers } from './store/team.state';
import { TeamsEffects } from './store/teams-effects';


@NgModule({
  imports: [
    StoreModule.forFeature(storeKey, reducers),
    EffectsModule.forFeature([TeamsEffects]),
  ]
})
export class TeamModule {}


















