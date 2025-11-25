import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import {
  BiaPermission,
  PermissionGuard,
} from 'packages/bia-ng/core/public-api';
import { RoleOptionModule } from 'packages/bia-ng/domains/public-api';
import {
  DynamicLayoutComponent,
  UserFromDirectoryModule,
} from 'packages/bia-ng/shared/public-api';
import { UserService } from './services/user.service';
import { FeatureUsersStore } from './store/user.state';
import { UsersEffects } from './store/users-effects';
import { userCRUDConfiguration } from './user.constants';
import { UserEditComponent } from './views/user-edit/user-edit.component';
import { UserImportComponent } from './views/user-import/user-import.component';
import { UserItemComponent } from './views/user-item/user-item.component';
import { UserNewComponent } from './views/user-new/user-new.component';
import { UsersIndexComponent } from './views/users-index/users-index.component';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: BiaPermission.User_List_Access,
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
          permission: BiaPermission.User_Save,
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
          permission: BiaPermission.User_Add,
          title: 'user.add',
        },
        component: UserNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: 'view',
        data: {
          featureViews: userCRUDConfiguration.featureName,
          featureConfiguration: userCRUDConfiguration,
          featureServiceType: UserService,
          leftWidth: 60,
        },
        loadChildren: () => import('../view.module').then(m => m.ViewModule),
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: false,
        },
        component: UserItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: BiaPermission.User_UpdateRoles,
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
export class BiaUserModule {}
