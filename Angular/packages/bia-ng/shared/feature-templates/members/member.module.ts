import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import {
  RoleOptionModule,
  UserOptionModule,
} from 'packages/bia-ng/domains/public-api';

import { UserFromDirectoryModule } from '../../features/users-from-directory/user-from-directory.module';
import { memberCRUDConfiguration } from './member.constants';
import { FeatureMembersStore } from './store/member.state';
import { MembersEffects } from './store/members-effects';

@NgModule({
  imports: [
    StoreModule.forFeature(
      memberCRUDConfiguration.storeKey,
      FeatureMembersStore.reducers
    ),
    EffectsModule.forFeature([MembersEffects]),
    // TODO after creation of CRUD Member : select the optioDto dommain module required for link
    // Domain Modules:
    UserOptionModule,
    RoleOptionModule,
    UserFromDirectoryModule,
  ],
})
export class MemberModule {}
