import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { BiaSignalRService } from 'src/app/core/bia-core/services/bia-signalr.service';
import { addUnreadNotification, removeUnreadNotification } from '../store/notifications-actions';
import { Notification } from '../model/notification';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';

/**
 * Service managing SignalR events for hangfire jobs.
 * To use it:
 * - Add a parameter of this type in the constructor of your component (for dependency injection)
 * - Call the 'initialize()' method on it, so that dependency injection is truly performed
 */
@Injectable()
export class NotificationSignalRService {
  /**
   * Constructor.
   * @param store the store.
   * @param signalRService the service managing the SignalR connection.
   */
  constructor(
    private store: Store<AppState>,
    private signalRService: BiaSignalRService,
    private authService: AuthService,
    private messageService: BiaMessageService) {
    // Do nothing.
  }

  /**
   * Initialize SignalR communication.
   * Note: this method has been created so that we have to call one method on this class, otherwise dependency injection is not working.
   */
  initialize() {
    this.signalRService.addMethod('notification-sent', (args) => {
      const notification: Notification = JSON.parse(args);
      var userInfo = this.authService.getAdditionalInfos();
      
      var okSite : Boolean =  notification.site.id == userInfo.userData.currentSiteId
      var okUser : Boolean =  (notification.notifiedUsers == undefined) || (notification.notifiedUsers.length == 0) || (notification.notifiedUsers.some(u => u.id==userInfo.userInfo.id))
      var okRole : Boolean =  (notification.notifiedRoles == undefined) || (notification.notifiedRoles.length == 0) || (notification.notifiedRoles.some(e => this.authService.hasPermission(e.display)))


      if 
      (
        okSite
        &&
        okUser
        &&
        okRole
      )
      {
        this.messageService.showInfo(notification.description);
        this.store.dispatch(addUnreadNotification({ id: notification.id }));
      }
    });

    this.signalRService.addMethod('notification-read', (id) => {
      console.log('%c [Notification] Notification Count', 'color: green; font-weight: bold');
      var idNum: number = +id;
      this.store.dispatch(removeUnreadNotification({ id: idNum }));
    });
  }

  destroy() {
    this.signalRService.removeMethod('notification-sent');
    this.signalRService.removeMethod('notification-count');
  }
}
