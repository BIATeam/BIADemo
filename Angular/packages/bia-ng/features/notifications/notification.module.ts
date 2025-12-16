import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { BiaPermission, PermissionGuard } from '@bia-team/bia-ng/core';
import {
  LanguageOptionModule,
  NotificationTypeOptionModule,
  RoleOptionModule,
  TeamOptionModule,
  UserOptionModule,
} from '@bia-team/bia-ng/domains';
import { DynamicLayoutComponent } from '@bia-team/bia-ng/shared';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { notificationCRUDConfiguration } from './notification.constants';
import { NotificationService } from './services/notification.service';
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
      permission: BiaPermission.Notification_List_Access,
      injectComponent: NotificationsIndexComponent,
      configuration: notificationCRUDConfiguration,
    },
    component: DynamicLayoutComponent,
    canActivate: [PermissionGuard],
    children: [
      {
        path: 'create',
        data: {
          breadcrumb: 'bia.add',
          canNavigate: false,
          permission: BiaPermission.Notification_Create,
          title: 'notification.add',
          style: {
            minWidth: '60vw',
            maxWidth: '60vw',
          },
        },
        component: NotificationNewComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: 'view',
        data: {
          featureConfiguration: notificationCRUDConfiguration,
          featureServiceType: NotificationService,
          leftWidth: 60,
        },
        loadChildren: () => import('../view.module').then(m => m.ViewModule),
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
              permission: BiaPermission.Notification_Update,
              title: 'notification.edit',
              style: {
                minWidth: '60vw',
                maxWidth: '60vw',
              },
            },
            component: NotificationEditComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: 'detail',
            data: {
              breadcrumb: 'bia.detail',
              canNavigate: true,
              permission: BiaPermission.Notification_Read,
              title: 'notification.detail',
            },
            component: NotificationDetailComponent,
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
export class BiaNotificationModule {}
