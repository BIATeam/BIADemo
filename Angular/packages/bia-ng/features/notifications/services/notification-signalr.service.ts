import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  AuthService,
  BiaSignalRService,
  CoreTeamsStore,
} from 'packages/bia-ng/core/public-api';
import { Team } from 'packages/bia-ng/models/public-api';
import { CrudItemSignalRService } from 'packages/bia-ng/shared/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { first } from 'rxjs/operators';
import { Notification } from '../model/notification';
import { NotificationListItem } from '../model/notification-list-item';
import { NotificationService } from './notification.service';

/**
 * SignalR service for the notification CRUD feature.
 *
 * Overrides the base CrudItemSignalRService to add user/team/role filtering:
 * only triggers a list refresh when the incoming notification actually targets
 * the current user, so unrelated notifications don't cause unnecessary reloads.
 *
 * Override `isInMyDisplay()` in a subclass to customize the filtering logic.
 */
@Injectable({
  providedIn: 'root',
})
export class NotificationsSignalRService extends CrudItemSignalRService<
  NotificationListItem,
  Notification
> {
  protected myTeams: Team[];

  constructor(
    protected override store: Store<BiaAppState>,
    protected override signalRService: BiaSignalRService,
    protected authService: AuthService,
    protected override injector: Injector
  ) {
    super(injector);
  }

  override initialize(crudItemService: NotificationService) {
    this.store.select(CoreTeamsStore.getAllTeams).subscribe(teams => {
      this.myTeams = teams;
    });

    console.log(
      '%c [Notifications] Register SignalR : refresh-notification',
      'color: purple; font-weight: bold'
    );

    this.signalRService.addMethod('refresh-notification', args => {
      const notification: Notification = JSON.parse(args);
      if (this.isInMyDisplay(notification)) {
        crudItemService.lastLazyLoadEvent$.pipe(first()).subscribe(event => {
          console.log(
            '%c [Notifications] RefreshSuccess',
            'color: green; font-weight: bold'
          );
          crudItemService.loadAllByPost(event);
        });
      }
    });

    this.signalRService.addMethod('refresh-notifications-several', args => {
      const notifications: Notification[] = JSON.parse(args);
      if (notifications.some(n => this.isInMyDisplay(n))) {
        crudItemService.lastLazyLoadEvent$.pipe(first()).subscribe(event => {
          console.log(
            '%c [Notifications] RefreshSuccess',
            'color: green; font-weight: bold'
          );
          crudItemService.loadAllByPost(event);
        });
      }
    });

    this.targetedFeature = { featureName: 'notifications' };
    this.signalRService.joinGroup(this.targetedFeature);
  }

  /**
   * Returns true if the notification targets the current user.
   * Override this in a subclass to customize filtering logic.
   */
  protected isInMyDisplay(notification: Notification): boolean {
    const decryptedToken = this.authService.getDecryptedToken();

    const okUser =
      !notification.notifiedUsers ||
      notification.notifiedUsers.length === 0 ||
      notification.notifiedUsers.some(u => u.id === decryptedToken.id);

    const okTeam =
      !notification.notifiedTeams ||
      notification.notifiedTeams.length === 0 ||
      notification.notifiedTeams.some(notifiedTeam =>
        this.myTeams.some(myTeam => {
          if (
            myTeam.id === notifiedTeam.team.id ||
            notifiedTeam.team.id === 0
          ) {
            if (notifiedTeam.roles && notifiedTeam.roles.length > 0) {
              return notifiedTeam.roles.some(notifiedRole =>
                myTeam.roles.some(myRole => myRole.id === notifiedRole.id)
              );
            }
            return true;
          }
          return false;
        })
      );

    return okUser && okTeam;
  }

  override destroy(crudItemService: NotificationService) {
    console.log(
      '%c [Notifications] Unregister SignalR : refresh-notification',
      'color: purple; font-weight: bold'
    );
    this.signalRService.removeMethod('refresh-notification');
    this.signalRService.removeMethod('refresh-notifications-several');
    this.signalRService.leaveGroup(this.targetedFeature);
  }
}
