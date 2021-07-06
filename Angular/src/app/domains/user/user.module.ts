import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/user.state';
import { UsersEffects } from './store/users-effects';

@NgModule({
  imports: [StoreModule.forFeature('domain-users', reducers), EffectsModule.forFeature([UsersEffects])]
})
export class UserModule {}
