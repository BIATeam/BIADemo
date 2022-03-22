import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { first } from 'rxjs/operators';
import { BiaSignalRService } from 'src/app/core/bia-core/services/bia-signalr.service';
import { loadAllByPost } from '../store/notifications-actions';
import { getLastLazyLoadEvent } from '../store/notification.state';
import { LazyLoadEvent } from 'primeng/api';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Notification } from '../model/notification';
import { TargetedFeature } from 'src/app/shared/bia-shared/model/signalR';
import { Team } from 'src/app/domains/team/model/team';
import { getAllTeams } from 'src/app/domains/team/store/team.state';

/**
 * Service managing SignalR events for hangfire jobs.
 * To use it:
 * - Add a parameter of this type in the constructor of your component (for dependency injection)
 * - Call the 'initialize()' method on it, so that dependency injection is truly performed
 */
@Injectable({
  providedIn: 'root'
})
export class NotificationsSignalRService {

  private targetedFeature: TargetedFeature;
  private myTeams: Team[];

  /**
   * Constructor.
   * @param store the store.
   * @param signalRService the service managing the SignalR connection.
   */
  constructor(
    private store: Store<AppState>,
    private signalRService: BiaSignalRService,
    private authService: AuthService) {
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

    console.log('%c [Notifications] Register SignalR : refresh-notifications', 'color: purple; font-weight: bold');
    this.signalRService.addMethod('refresh-notifications', (args) => {
      const notification: Notification = JSON.parse(args);
      if (this.IsInMyDisplay(notification)) {
        this.store.select(getLastLazyLoadEvent).pipe(first()).subscribe(
          (event) => {
            console.log('%c [Notifications] RefreshSuccess', 'color: green; font-weight: bold');
            this.store.dispatch(loadAllByPost({ event: <LazyLoadEvent>event }));
          }
        );
      }
    });
    this.signalRService.addMethod('refresh-notifications-several', (args) => {
      const notifications: Notification[] = JSON.parse(args);
      if (notifications.some(notification => this.IsInMyDisplay(notification))) {
        this.store.select(getLastLazyLoadEvent).pipe(first()).subscribe(
          (event) => {
            console.log('%c [Notifications] RefreshSuccess', 'color: green; font-weight: bold');
            this.store.dispatch(loadAllByPost({ event: <LazyLoadEvent>event }));
          }
        );
      }
    });
    this.targetedFeature = { featureName: 'notifications' };
    this.signalRService.joinGroup(this.targetedFeature);
  }
  private IsInMyDisplay(notification: Notification) {
    const additionalInfo = this.authService.getAdditionalInfos();
    const okUser: Boolean = (notification.notifiedUsers === undefined) ||
      (notification.notifiedUsers === null) ||
      (notification.notifiedUsers.length === 0) ||
      (notification.notifiedUsers.some(u => u.id === additionalInfo.userInfo.id));

    const okTeam: Boolean = !notification.notifiedTeams ||
      (notification.notifiedTeams.length === 0) ||
      (notification.notifiedTeams.some(notifiedTeam => this.myTeams.some(myTeam => myTeam.id === notifiedTeam.id)));

    return okUser && okTeam;
  }
  destroy() {
    console.log('%c [Notifications] Unregister SignalR : refresh-notifications', 'color: purple; font-weight: bold');
    this.signalRService.removeMethod('refresh-notifications');
    this.signalRService.leaveGroup(this.targetedFeature);
  }
}
