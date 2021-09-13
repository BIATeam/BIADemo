import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { first } from 'rxjs/operators';
import { BiaSignalRService } from 'src/app/core/bia-core/services/bia-signalr.service';
import { loadAllByPost } from '../store/notifications-actions';
import { getLastLazyLoadEvent } from '../store/notification.state';
import { LazyLoadEvent } from 'primeng/api';

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
  /**
   * Constructor.
   * @param store the store.
   * @param signalRService the service managing the SignalR connection.
   */
  constructor(private store: Store<AppState>, private signalRService: BiaSignalRService) {
    // Do nothing.
  }

  /**
   * Initialize SignalR communication.
   * Note: this method has been created so that we have to call one method on this class, otherwise dependency injection is not working.
   */
  initialize() {
    console.log('%c [Notifications] Register SignalR : refresh-notifications', 'color: purple; font-weight: bold');
    this.signalRService.addMethod('refresh-notifications', () => {
      this.store.select(getLastLazyLoadEvent).pipe(first()).subscribe(
        (event) => {
          console.log('%c [Notifications] RefreshSuccess', 'color: green; font-weight: bold');
          this.store.dispatch(loadAllByPost({ event: <LazyLoadEvent>event }));
        }
      );
    });
  }

  destroy() {
    console.log('%c [Notifications] Unregister SignalR : refresh-notifications', 'color: purple; font-weight: bold');
    this.signalRService.removeMethod('refresh-notifications');
  }
}
