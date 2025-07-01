import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { RoleOptionModule } from 'src/app/domains/bia-domains/role-option/role-option.module';
import { UserOptionModule } from 'src/app/domains/bia-domains/user-option/user-option.module';
import { UserFromDirectoryModule } from 'src/app/features/bia-features/users-from-directory/user-from-directory.module';
import { memberCRUDConfiguration } from './member.constants';
import { FeatureMembersStore } from './store/member.state';
import { MembersEffects } from './store/members-effects';

@NgModule({
  imports: [
    // RouterModule.forChild(ROUTES),
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
