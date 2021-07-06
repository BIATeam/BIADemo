import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/user-from-AD.state';
import { UsersFromADEffects } from './store/users-from-AD-effects';

@NgModule({
  imports: [StoreModule.forFeature('domain-users-from-AD', reducers), EffectsModule.forFeature([UsersFromADEffects])]
})
export class UserFromADModule {}
