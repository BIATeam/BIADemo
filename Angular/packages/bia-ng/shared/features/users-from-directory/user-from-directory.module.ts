import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import {
  LdapDomainModule,
  UserOptionModule,
} from 'packages/bia-ng/domains/public-api';

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
