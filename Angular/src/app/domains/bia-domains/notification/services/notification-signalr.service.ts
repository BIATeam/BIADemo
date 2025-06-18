import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaSignalRService } from 'src/app/core/bia-core/services/bia-signalr.service';
import { TargetedFeature } from 'src/app/shared/bia-shared/model/signalR';
import { AppState } from 'src/app/store/state';
import { Team } from '../../team/model/team';
import { getAllTeams } from '../../team/store/team.state';
import { Notification } from '../model/notification';
import { DomainNotificationsActions } from '../store/notifications-actions';

/**
 * Service managing SignalR events for hangfire jobs.
 * To use it:
 * - Add a parameter of this type in the constructor of your component (for dependency injection)
 * - Call the 'initialize()' method on it, so that dependency injection is truly performed
 */
@Injectable()
export class NotificationSignalRService {
  protected targetedFeature: TargetedFeature;
  protected myTeams: Team[];

  /**
   * Constructor.
   * @param store the store.
   * @param signalRService the service managing the SignalR connection.
   */
  constructor(
    protected store: Store<AppState>,
    protected signalRService: BiaSignalRService,
    protected authService: AuthService,
    protected messageService: BiaMessageService
  ) {}

  /**
   * Initialize SignalR communication.
   * Note: this method has been created so that we have to call one method on this class, otherwise dependency injection is not working.
   */
  initialize() {
    this.store.select(getAllTeams).subscribe(teams => {
      this.myTeams = teams;
    });

    this.signalRService.addMethod('notification-addUnread', args => {
      const notification: Notification = JSON.parse(args);
      notification.data = notification.jData
        ? JSON.parse(notification.jData)
        : { route: null, display: '', teams: null };
      if (this.isInMyDisplay(notification)) {
        this.messageService.showNotification(notification);
        this.store.dispatch(
          DomainNotificationsActions.addUnreadNotification({
            id: notification.id,
          })
        );
      }
    });

    this.signalRService.addMethod('notification-removeUnread', id => {
      console.log(
        '%c [Notification] Notification Count',
        'color: green; font-weight: bold'
      );
      const idNum: number = +id;
      this.store.dispatch(
        DomainNotificationsActions.removeUnreadNotification({ id: idNum })
      );
    });

    this.signalRService.addMethod('notification-removeSeveralUnread', args => {
      const ids: number[] = JSON.parse(args);
      console.log(
        '%c [Notification] Notification Count',
        'color: green; font-weight: bold'
      );
      ids.forEach(idNum =>
        this.store.dispatch(
          DomainNotificationsActions.removeUnreadNotification({ id: idNum })
        )
      );
    });

    this.targetedFeature = { featureName: 'notification-domain' };
    this.signalRService.joinGroup(this.targetedFeature);
  }

  protected isInMyDisplay(notification: Notification) {
    const decryptedToken = this.authService.getDecryptedToken();

    // OK if no notifiedUsers are specified or if the current user is amongst the notifiedUsers
    const okUser: boolean =
      notification.notifiedUsers &&
      notification.notifiedUsers.length >= 0 &&
      notification.notifiedUsers.some(u => u.id === decryptedToken.id);

    // OK if no notifiedTeams are specified or if the current user is part of one of the notifiedTeams.
    // If the notifiedTeam targets specific roles, the current user must have one of these roles assigned in the given team
    const okTeam: boolean =
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
          } else {
            return false;
          }
        })
      );

    // V5: see nominative notification even if not in the team or role
    const noTeamAndNoUser: boolean =
      (!notification.notifiedUsers ||
        notification.notifiedUsers.length === 0) &&
      (!notification.notifiedTeams || notification.notifiedTeams.length === 0);

    return noTeamAndNoUser || okUser || okTeam;
  }

  destroy() {
    this.signalRService.removeMethod('notification-addUnread');
    this.signalRService.removeMethod('notification-removeUnread');
    this.signalRService.leaveGroup(this.targetedFeature);
  }
}
