import { NgModule } from '@angular/core';
import { LdapDomainModule, UserOptionModule } from '@bia-team/bia-ng/domains';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';

import { UsersFromDirectoryStore } from './store/user-from-directory.state';
import { UsersFromDirectoryEffects } from './store/users-from-directory-effects';

@NgModule({
  imports: [
    StoreModule.forFeature(
      'users-from-directory',
      UsersFromDirectoryStore.reducers
    ),
    EffectsModule.forFeature([UsersFromDirectoryEffects]),
    UserOptionModule, // required for synchronization when user added
    LdapDomainModule,
  ],
})
export class UserFromDirectoryModule {}
