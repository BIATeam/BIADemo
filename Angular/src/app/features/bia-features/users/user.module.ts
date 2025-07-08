import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { RoleOptionModule } from 'src/app/domains/bia-domains/role-option/role-option.module';
import { DynamicLayoutComponent } from 'src/app/shared/bia-shared/components/layout/dynamic-layout/dynamic-layout.component';
import { Permission } from 'src/app/shared/permission';
import { UserFromDirectoryModule } from '../users-from-directory/user-from-directory.module';
import { FeatureUsersStore } from './store/user.state';
import { UsersEffects } from './store/users-effects';
import { userCRUDConfiguration } from './user.constants';
import { UserEditComponent } from './views/user-edit/user-edit.component';
import { UserImportComponent } from './views/user-import/user-import.component';
import { UserItemComponent } from './views/user-item/user-item.component';
import { UserNewComponent } from './views/user-new/user-new.component';
import { UsersIndexComponent } from './views/users-index/users-index.component';

export const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.User_List_Access,
      injectComponent: UsersIndexComponent,
      configuration: userCRUDConfiguration,
    },
    component: DynamicLayoutComponent,
    canActivate: [PermissionGuard],
    // [Calc] : The children are not used in calc
    children: [
      {
        path: 'import',
        data: {
          breadcrumb: 'user.import',
          canNavigate: false,
          style: {
            minWidth: '80vw',
            maxWidth: '80vw',
            maxHeight: '80vh',
          },
          permission: Permission.User_Save,
          title: 'user.import',
        },
        component: UserImportComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: 'create',
        data: {
          breadcrumb: 'bia.add',
          canNavigate: false,
          permission: Permission.User_Add,
          title: 'user.add',
        },
        component: UserNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: true,
        },
        component: UserItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.User_UpdateRoles,
              title: 'user.edit',
            },
            component: UserEditComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
        ],
      },
    ],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature(
      userCRUDConfiguration.storeKey,
      FeatureUsersStore.reducers
    ),
    EffectsModule.forFeature([UsersEffects]),
    // TODO after creation of CRUD User : select the optioDto dommain module required for link
    // Domain Modules:
    RoleOptionModule,
    UserFromDirectoryModule,
  ],
})
export class UserModule {}
