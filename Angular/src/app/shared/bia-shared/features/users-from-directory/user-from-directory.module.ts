import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from 'src/app/shared/shared.module';
import { UserFromLdapFormComponent } from './components/user-from-directory-form/user-from-directory-form.component';
import { reducers } from './store/user-from-Directory.state';
import { UsersFromDirectoryEffects } from './store/users-from-Directory-effects';
import { UserAddFromLdapComponent } from './views/user-add-from-directory-dialog/user-add-from-directory-dialog.component';

const FEATURES = [
  UserAddFromLdapComponent
]

const USER_FROM_DIRECTORY_COMPONENTS = [
  UserAddFromLdapComponent,
  UserFromLdapFormComponent
];

@NgModule({
  imports: [SharedModule, StoreModule.forFeature('users-from-directory', reducers), EffectsModule.forFeature([UsersFromDirectoryEffects])],
  declarations: [...USER_FROM_DIRECTORY_COMPONENTS],
  exports: [...FEATURES]
})
export class UserFromDirectoryModule {}
