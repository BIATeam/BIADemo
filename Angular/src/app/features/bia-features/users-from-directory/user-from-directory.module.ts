import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { LdapDomainModule } from 'src/app/domains/bia-domains/ldap-domain/ldap-domain.module';
import { UserOptionModule } from 'src/app/domains/bia-domains/user-option/user-option.module';

import { reducers } from './store/user-from-directory.state';
import { UsersFromDirectoryEffects } from './store/users-from-directory-effects';

@NgModule({
  imports: [
    StoreModule.forFeature('users-from-directory', reducers),
    EffectsModule.forFeature([UsersFromDirectoryEffects]),
    UserOptionModule, // required for synchronization when user added
    LdapDomainModule,
  ],
})
export class UserFromDirectoryModule {}
