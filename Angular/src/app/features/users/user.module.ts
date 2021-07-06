import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { UsersEffects } from './store/users-effects';
import { reducers } from './store/user.state';
import { UserFormComponent } from './components/user-form/user-form.component';
import { UsersIndexComponent } from './views/users-index/users-index.component';
import { UserNewDialogComponent } from './views/user-new-dialog/user-new-dialog.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { reducers as usersReducers } from 'src/app/domains/user/store/user.state';
import { UsersEffects as DomainUsersEffects } from 'src/app/domains/user/store/users-effects';
import { reducers as usersFromADReducers } from 'src/app/domains/user-from-AD/store/user-from-AD.state';
import { reducers as ldapDomainsReducer } from 'src/app/domains/ldap-domain/store/ldap-domain.state';
import { UsersFromADEffects as DomainUsersFromADEffects } from 'src/app/domains/user-from-AD/store/users-from-AD-effects';

import { UserTableHeaderComponent } from './components/user-table-header/user-table-header.component';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { Permission } from 'src/app/shared/permission';
import { LdapDomainsEffects } from 'src/app/domains/ldap-domain/store/ldap-domain-effects';
import { LdapDomainModule } from 'src/app/domains/ldap-domain/ldap-domain.module';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.User_List_Access
    },
    component: UsersIndexComponent,
    canActivate: [PermissionGuard]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [UserFormComponent, UserNewDialogComponent, UsersIndexComponent, UserTableHeaderComponent],
  entryComponents: [UserNewDialogComponent],
  imports: [
    SharedModule,
    LdapDomainModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('users', reducers),
    StoreModule.forFeature('domain-users', usersReducers),
    StoreModule.forFeature('domain-users-from-AD', usersFromADReducers),
    StoreModule.forFeature('domain-ldap-domains', ldapDomainsReducer),
    EffectsModule.forFeature([UsersEffects]),
    EffectsModule.forFeature([DomainUsersEffects]),
    EffectsModule.forFeature([DomainUsersFromADEffects]),
    EffectsModule.forFeature([LdapDomainsEffects])
  ]
})
export class UserModule {}
