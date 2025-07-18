import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { Permission, PermissionGuard } from 'biang/core';
import {
  LanguageOptionModule,
  NotificationTypeOptionModule,
  RoleOptionModule,
  TeamOptionModule,
  UserOptionModule,
} from 'biang/domains';
import { FullPageLayoutComponent, PopupLayoutComponent } from 'biang/shared';
import { FeatureNotificationsStore } from './store/notification.state';
import { NotificationsEffects } from './store/notifications-effects';
import { NotificationDetailComponent } from './views/notification-detail/notification-detail.component';
import { NotificationEditComponent } from './views/notification-edit/notification-edit.component';
import { NotificationItemComponent } from './views/notification-item/notification-item.component';
import { NotificationNewComponent } from './views/notification-new/notification-new.component';
import { NotificationsIndexComponent } from './views/notifications-index/notifications-index.component';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Notification_List_Access,
      injectComponent: NotificationsIndexComponent,
    },
    component: FullPageLayoutComponent,
    canActivate: [PermissionGuard],
    children: [
      {
        path: 'create',
        data: {
          breadcrumb: 'bia.add',
          canNavigate: false,
          permission: Permission.Notification_Create,
          title: 'notification.add',
          injectComponent: NotificationNewComponent,
        },
        component: PopupLayoutComponent,
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
              injectComponent: NotificationEditComponent,
            },
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
              injectComponent: NotificationDetailComponent,
            },
            component: PopupLayoutComponent,
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
    ReactiveFormsModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('notifications', FeatureNotificationsStore.reducers),
    EffectsModule.forFeature([NotificationsEffects]),
    // Domain Modules:
    NotificationTypeOptionModule,
    UserOptionModule,
    LanguageOptionModule,
    RoleOptionModule,
    TeamOptionModule,
  ],
})
export class NotificationModule {}
