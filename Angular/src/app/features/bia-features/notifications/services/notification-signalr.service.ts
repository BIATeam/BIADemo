import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { first } from 'rxjs/operators';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaSignalRService } from 'src/app/core/bia-core/services/bia-signalr.service';
import { Team } from 'src/app/domains/bia-domains/team/model/team';
import { getAllTeams } from 'src/app/domains/bia-domains/team/store/team.state';
import { TargetedFeature } from 'src/app/shared/bia-shared/model/signalR';
import { AppState } from 'src/app/store/state';
import { Notification } from '../model/notification';
import { getLastLazyLoadEvent } from '../store/notification.state';
import { FeatureNotificationsActions } from '../store/notifications-actions';

/**
 * Service managing SignalR events for hangfire jobs.
 * To use it:
 * - Add a parameter of this type in the constructor of your component (for dependency injection)
 * - Call the 'initialize()' method on it, so that dependency injection is truly performed
 */
@Injectable({
  providedIn: 'root',
})
export class NotificationsSignalRService {
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
    protected authService: AuthService
  ) {
    // Do nothing.
  }

  /**
   * Initialize SignalR communication.
   * Note: this method has been created so that we have to call one method on this class, otherwise dependency injection is not working.
   */
  initialize() {
    this.store.select(getAllTeams).subscribe(teams => {
      this.myTeams = teams;
    });

    console.log(
      '%c [Notifications] Register SignalR : refresh-notification',
      'color: purple; font-weight: bold'
    );
    this.signalRService.addMethod('refresh-notification', args => {
      const notification: Notification = JSON.parse(args);
      if (this.isInMyDisplay(notification)) {
        this.store
          .select(getLastLazyLoadEvent)
          .pipe(first())
          .subscribe(event => {
            console.log(
              '%c [Notifications] RefreshSuccess',
              'color: green; font-weight: bold'
            );
            this.store.dispatch(
              FeatureNotificationsActions.loadAllByPost({
                event: event,
              })
            );
          });
      }
    });
    this.signalRService.addMethod('refresh-notifications-several', args => {
      const notifications: Notification[] = JSON.parse(args);
      if (
        notifications.some(notification => this.isInMyDisplay(notification))
      ) {
        this.store
          .select(getLastLazyLoadEvent)
          .pipe(first())
          .subscribe(event => {
            console.log(
              '%c [Notifications] RefreshSuccess',
              'color: green; font-weight: bold'
            );
            this.store.dispatch(
              FeatureNotificationsActions.loadAllByPost({
                event: event,
              })
            );
          });
      }
    });
    this.targetedFeature = { featureName: 'notifications' };
    this.signalRService.joinGroup(this.targetedFeature);
  }

  protected isInMyDisplay(notification: Notification) {
    const additionalInfo = this.authService.getAdditionalInfos();

    // OK if no notifiedUsers are specified or if the current user is amongst the notifiedUsers
    const okUser: boolean =
      !notification.notifiedUsers ||
      notification.notifiedUsers.length === 0 ||
      notification.notifiedUsers.some(u => u.id === additionalInfo.userInfo.id);

    // OK if no notifiedTeams are specified or if the current user is part of one of the notifiedTeams.
    // If the notifiedTeam targets specific roles, the current user must have one of these roles assigned in the given team
    const okTeam: boolean =
      !notification.notifiedTeams ||
      notification.notifiedTeams.length === 0 ||
      notification.notifiedTeams.some(notifiedTeam =>
        this.myTeams.some(myTeam => {
          if (myTeam.id === notifiedTeam.team.id || notifiedTeam.team.id == 0) {
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

    return okUser && okTeam;
  }

  destroy() {
    console.log(
      '%c [Notifications] Unregister SignalR : refresh-notification',
      'color: purple; font-weight: bold'
    );
    this.signalRService.removeMethod('refresh-notification');
    this.signalRService.removeMethod('refresh-notification');
    this.signalRService.leaveGroup(this.targetedFeature);
  }
}
