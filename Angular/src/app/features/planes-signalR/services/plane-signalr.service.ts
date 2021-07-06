import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { first } from 'rxjs/operators';
import { BiaSignalRService } from 'src/app/core/bia-core/services/bia-signalr.service';
import { loadAllByPost } from '../store/planes-actions';
import { getLastLazyLoadEvent } from '../store/plane.state';
import { LazyLoadEvent } from 'primeng/api';

/**
 * Service managing SignalR events for hangfire jobs.
 * To use it:
 * - Add a parameter of this type in the constructor of your component (for dependency injection)
 * - Call the 'initialize()' method on it, so that dependency injection is truly performed
 */
@Injectable()
export class PlanesSignalRService {
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
    console.log('%c [PlanesSignalR] Register SignalR : refresh-planesSignalR', 'color: purple; font-weight: bold');
    this.signalRService.addMethod('refresh-planesSignalR', () => {
      this.store.select(getLastLazyLoadEvent).pipe(first()).subscribe(
        (event) => {
          console.log('%c [PlanesSignalR] RefreshSuccess', 'color: green; font-weight: bold');
          this.store.dispatch(loadAllByPost({ event: <LazyLoadEvent>event }));
        }
      );
    });
  }

  destroy() {
    console.log('%c [PlanesSignalR] Unregister refresh-planesSignalR', 'color: purple; font-weight: bold');
    this.signalRService.removeMethod('refresh-planesSignalR');
  }
}
