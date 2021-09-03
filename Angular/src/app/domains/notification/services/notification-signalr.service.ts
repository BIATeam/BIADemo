import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { BiaSignalRService } from 'src/app/core/bia-core/services/bia-signalr.service';
import { loadAllNotifications } from '../store/notifications-actions';
import { Notification } from '../model/notification';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';

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
    private messageService: BiaMessageService) {
    // Do nothing.
  }

  /**
   * Initialize SignalR communication.
   * Note: this method has been created so that we have to call one method on this class, otherwise dependency injection is not working.
   */
  initialize() {
    console.log('%c [Notification] Register SignalR : notification-sent', 'color: purple; font-weight: bold');
    this.signalRService.addMethod('notification-sent', (args) => {

      const notification: Notification = JSON.parse(args);

      this.messageService.showInfo(notification.description);

      console.log('%c [Notification] Notification Sent', 'color: green; font-weight: bold');
      console.log(args);
      this.store.dispatch(loadAllNotifications());
    });
  }

  destroy() {
    console.log('%c [Notification] Unregister SignalR : notification-sent', 'color: purple; font-weight: bold');
    this.signalRService.removeMethod('notification-sent');
  }
}
