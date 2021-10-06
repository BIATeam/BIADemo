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
import { UserTableHeaderComponent } from './components/user-table-header/user-table-header.component';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { Permission } from 'src/app/shared/permission';
import { LdapDomainModule } from 'src/app/domains/ldap-domain/ldap-domain.module';
import { UserFromADModule } from 'src/app/domains/user-from-AD/user-from-AD.module';

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
    UserFromADModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('users', reducers),
    EffectsModule.forFeature([UsersEffects]),
  ]
})
export class UserModule {}
