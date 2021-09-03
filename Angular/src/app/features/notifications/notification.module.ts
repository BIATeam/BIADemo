import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { NotificationsEffects } from './store/notifications-effects';
import { reducers } from './store/notification.state';
import { NotificationsIndexComponent } from './views/notifications-index/notifications-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { Permission } from 'src/app/shared/permission';
import { NotificationSignalRService } from 'src/app/domains/notification/services/notification-signalr.service';
import { NotificationEditComponent } from './views/notification-edit/notification-edit.component';
import { NotificationFormComponent } from './components/notification-form/notification-form.component';
import { AsPipe } from './components/notification-form/as.pipe';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Notification_List_Access
    },
    component: NotificationsIndexComponent,
    canActivate: [PermissionGuard]
  },
  {
    path: 'edit',
    data: {
      breadcrumb: null,
      permission: Permission.Notification_Read
    },
    component: NotificationEditComponent,
    canActivate: [PermissionGuard]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [
    NotificationsIndexComponent,
    NotificationEditComponent,
    NotificationFormComponent,
    AsPipe],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('notifications', reducers),
    EffectsModule.forFeature([NotificationsEffects]),
  ],
  providers: [
    NotificationSignalRService,
    AsPipe
  ]
})
export class NotificationModule { }
