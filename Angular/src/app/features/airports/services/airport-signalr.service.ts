import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { first } from 'rxjs/operators';
import { BiaSignalRService } from 'src/app/core/bia-core/services/bia-signalr.service';
import { loadAllByPost } from '../store/airports-actions';
import { getLastLazyLoadEvent } from '../store/airport.state';
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
export class AirportsSignalRService {
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
    console.log('%c [Airports] Register SignalR : refresh-airports', 'color: purple; font-weight: bold');
    this.signalRService.addMethod('refresh-airports', () => {
      this.store.select(getLastLazyLoadEvent).pipe(first()).subscribe(
        (event) => {
          console.log('%c [Airports] RefreshSuccess', 'color: green; font-weight: bold');
          this.store.dispatch(loadAllByPost({ event: <LazyLoadEvent>event }));
        }
      );
    });
    this.signalRService.joinGroup("airports");
  }

  destroy() {
    console.log('%c [Airports] Unregister SignalR : refresh-airports', 'color: purple; font-weight: bold');
    this.signalRService.removeMethod('refresh-airports');
    this.signalRService.leaveGroup("airports");
  }
}
