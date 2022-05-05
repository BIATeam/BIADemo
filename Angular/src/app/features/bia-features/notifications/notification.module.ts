import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { NotificationsEffects } from './store/notifications-effects';
import { reducers } from './store/notification.state';
import { NotificationFormComponent } from './components/notification-form/notification-form.component';
import { NotificationsIndexComponent } from './views/notifications-index/notifications-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { NotificationNewComponent } from './views/notification-new/notification-new.component';
import { NotificationEditComponent } from './views/notification-edit/notification-edit.component';
import { Permission } from 'src/app/shared/permission';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { NotificationItemComponent } from './views/notification-item/notification-item.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { NotificationTableComponent } from './components/notification-table/notification-table.component';
import { UserOptionModule } from 'src/app/domains/bia-domains/user-option/user-option.module';
import { NotificationTypeOptionModule } from 'src/app/domains/bia-domains/notification-type-option/notification-type-option.module';
import { NotificationDetailComponent } from './views/notification-detail/notification-detail.component';
import { ReactiveFormsModule } from '@angular/forms';
import { LanguageOptionModule } from 'src/app/domains/bia-domains/language-option/language-option.module';
import { RoleOptionModule } from 'src/app/domains/bia-domains/role-option/role-option.module';
import { TeamOptionModule } from 'src/app/domains/bia-domains/team-option/team-option.module';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Notification_List_Access,
      InjectComponent: NotificationsIndexComponent
    },
    component: FullPageLayoutComponent,
    canActivate: [PermissionGuard],
    // [Calc] : The children are not used in calc
    children: [
      {
        path: 'create',
        data: {
          breadcrumb: 'bia.add',
          canNavigate: false,
          permission: Permission.Notification_Create,
          title: 'notification.add',
          InjectComponent: NotificationNewComponent,
        },
        component: PopupLayoutComponent,
        // component: FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':notificationId',
        data: {
          breadcrumb: '',
          canNavigate: true,
        },
        component: NotificationItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.Notification_Update,
              title: 'notification.edit',
              InjectComponent: NotificationEditComponent,
            },
            // component: PopupLayoutComponent,
            component: FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: 'detail',
            data: {
              breadcrumb: 'bia.detail',
              canNavigate: true,
              permission: Permission.Notification_Read,
              title: 'notification.detail',
              InjectComponent: NotificationDetailComponent,
            },
            component: PopupLayoutComponent,
            // component: FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            redirectTo: 'edit'
          },
        ]
      },
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [
    NotificationItemComponent,
    // [Calc] : NOT used only for calc (4 lines).
    // it is possible to delete unsed commponent files (views/..-new + views/..-edit + components/...-form).
    NotificationFormComponent,
    NotificationsIndexComponent,
    NotificationNewComponent,
    NotificationEditComponent,
    NotificationDetailComponent,
    // [Calc] : Used only for calc it is possible to delete unsed commponent files (components/...-table)).
    NotificationTableComponent,
  ],
  imports: [
    SharedModule,
    ReactiveFormsModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('notifications', reducers),
    EffectsModule.forFeature([NotificationsEffects]),
    // Domain Modules:
    NotificationTypeOptionModule,
    UserOptionModule,
    LanguageOptionModule,
    RoleOptionModule,
    TeamOptionModule,
  ]
})
export class NotificationModule {
}

