import { ModuleWithProviders, NgModule, Provider, Type } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ROUTES, RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import {
  BiaPermission,
  PermissionGuard,
} from 'packages/bia-ng/core/public-api';
import {
  LanguageOptionModule,
  NotificationTypeOptionModule,
  RoleOptionModule,
  TeamOptionModule,
  UserOptionModule,
} from 'packages/bia-ng/domains/public-api';
import {
  CrudConfig,
  CrudItemService,
  DynamicLayoutComponent,
} from 'packages/bia-ng/shared/public-api';
import { NotificationListItem } from './model/notification-list-item';
import { notificationCRUDConfiguration } from './notification.constants';
import { NotificationDas } from './services/notification-das.service';
import { NotificationOptionsService } from './services/notification-options.service';
import { NotificationsSignalRService } from './services/notification-signalr.service';
import { NotificationService } from './services/notification.service';
import { FeatureNotificationsStore } from './store/notification.state';
import {
  NOTIFICATION_CRUD_CONFIG,
  NotificationsEffects,
} from './store/notifications-effects';
import { NotificationDetailComponent } from './views/notification-detail/notification-detail.component';
import { NotificationEditComponent } from './views/notification-edit/notification-edit.component';
import { NotificationItemComponent } from './views/notification-item/notification-item.component';
import { NotificationNewComponent } from './views/notification-new/notification-new.component';
import { NotificationsIndexComponent } from './views/notifications-index/notifications-index.component';

export interface BiaNotificationModuleConfig {
  /** Override the index component. Defaults to NotificationsIndexComponent. */
  indexComponent?: Type<unknown>;
  /** Override the detail component. Defaults to NotificationDetailComponent. */
  detailComponent?: Type<unknown>;
  /** Override the edit component. Defaults to NotificationEditComponent. */
  editComponent?: Type<unknown>;
  /** Override the new/create component. Defaults to NotificationNewComponent. */
  newComponent?: Type<unknown>;
  /** Override the item shell component. Defaults to NotificationItemComponent. */
  itemComponent?: Type<unknown>;
  /** Override the CRUD configuration (e.g. to add columns). Defaults to notificationCRUDConfiguration. */
  crudConfiguration?: CrudConfig<NotificationListItem>;
  /** Override the service providers (DAS, SignalR, Options, Service). */
  providers?: Provider[];
}

export function buildNotificationRoutes(
  config: BiaNotificationModuleConfig = {}
): Routes {
  const indexComponent = config.indexComponent ?? NotificationsIndexComponent;
  const detailComponent = config.detailComponent ?? NotificationDetailComponent;
  const editComponent = config.editComponent ?? NotificationEditComponent;
  const newComponent = config.newComponent ?? NotificationNewComponent;
  const itemComponent = config.itemComponent ?? NotificationItemComponent;
  const crudConfig = config.crudConfiguration ?? notificationCRUDConfiguration;

  return [
    {
      path: '',
      data: {
        breadcrumb: null,
        permission: BiaPermission.Notification_List_Access,
        injectComponent: indexComponent,
        configuration: crudConfig,
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
            style: { minWidth: '60vw', maxWidth: '60vw' },
          },
          component: newComponent,
          canActivate: [PermissionGuard],
        },
        {
          path: 'view',
          data: {
            featureConfiguration: crudConfig,
            featureServiceType: NotificationService,
            leftWidth: 60,
          },
          loadChildren: () => import('../view.module').then(m => m.ViewModule),
        },
        {
          path: ':notificationId',
          data: { breadcrumb: '', canNavigate: true },
          component: itemComponent,
          canActivate: [PermissionGuard],
          children: [
            {
              path: 'edit',
              data: {
                breadcrumb: 'bia.edit',
                canNavigate: true,
                permission: BiaPermission.Notification_Update,
                title: 'notification.edit',
                style: { minWidth: '60vw', maxWidth: '60vw' },
              },
              component: editComponent,
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
              component: detailComponent,
              canActivate: [PermissionGuard],
            },
            { path: '', pathMatch: 'full', redirectTo: 'edit' },
          ],
        },
      ],
    },
    { path: '**', redirectTo: '' },
  ];
}

@NgModule({
  imports: [
    ReactiveFormsModule,
    RouterModule.forChild(buildNotificationRoutes()),
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
export class BiaNotificationModule {
  /**
   * Use this in your app feature module to customize the notification feature
   * without duplicating the entire module.
   *
   * @example
   * BiaNotificationModule.forFeature({
   *   crudConfiguration: specificNotificationCRUDConfiguration,
   *   providers: [
   *     { provide: NotificationDas, useClass: SpecificNotificationDas },
   *     { provide: NotificationService, useClass: SpecificNotificationService },
   *     { provide: CrudItemService, useExisting: NotificationService },
   *   ],
   * })
   */
  static forFeature(
    config: BiaNotificationModuleConfig = {}
  ): ModuleWithProviders<BiaNotificationFeatureModule> {
    return {
      ngModule: BiaNotificationFeatureModule,
      providers: [
        // Routes are provided via the ROUTES token so the config is captured
        // at forFeature() call time, not at module decoration time.
        {
          provide: ROUTES,
          useValue: buildNotificationRoutes(config),
          multi: true,
        },
        // Provide the crud config to effects so they use the correct useSignalR flag.
        ...(config.crudConfiguration
          ? [
              {
                provide: NOTIFICATION_CRUD_CONFIG,
                useValue: config.crudConfiguration,
              },
            ]
          : []),
        ...(config.providers ?? []),
      ],
    };
  }
}

/**
 * Feature module created by BiaNotificationModule.forFeature().
 * Uses the ROUTES token so routes are built with the caller's config.
 */
@NgModule({
  imports: [
    ReactiveFormsModule,
    RouterModule.forChild([]), // empty — actual routes injected via ROUTES token above
    StoreModule.forFeature('notifications', FeatureNotificationsStore.reducers),
    EffectsModule.forFeature([NotificationsEffects]),
    NotificationTypeOptionModule,
    UserOptionModule,
    LanguageOptionModule,
    RoleOptionModule,
    TeamOptionModule,
  ],
  providers: [
    NotificationDas,
    NotificationOptionsService,
    NotificationsSignalRService,
    NotificationService,
    { provide: CrudItemService, useExisting: NotificationService },
  ],
})
export class BiaNotificationFeatureModule {}
