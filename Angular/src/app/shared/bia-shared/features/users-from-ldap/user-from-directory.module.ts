import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/user-from-Directory.state';
import { UsersFromDirectoryEffects } from './store/users-from-Directory-effects';

@NgModule({
  imports: [StoreModule.forFeature('users-from-directory', reducers), EffectsModule.forFeature([UsersFromDirectoryEffects])]
})
export class UserFromDirectoryModule {}
