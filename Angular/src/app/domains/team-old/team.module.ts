import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/team.state';
import { TeamsEffects } from './store/teams-effects';


@NgModule({
  imports: [
    StoreModule.forFeature('domain-teams', reducers),
    EffectsModule.forFeature([TeamsEffects]),
  ],
  providers: []
})
export class TeamModule {}


















