import { Injectable, Injector } from '@angular/core';
import {
  CrudItemService,
  CrudItemSignalRService,
} from 'packages/bia-ng/shared/public-api';
import { Airport } from '../model/airport';

/**
 * Service managing SignalR events for airports.
 * Extends CrudItemSignalRService to handle both:
 * - List refresh when any airport changes (generic functionality)
 * - Item update notifications for edit detection (airport-specific functionality)
 *
 * To use it:
 * - Add a parameter of this type in the constructor of your component (for dependency injection)
 * - Call the 'initialize()' method on it, so that dependency injection is truly performed
 */
@Injectable({
  providedIn: 'root',
})
export class AirportSignalRService extends CrudItemSignalRService<Airport> {
  /**
   * Constructor.
   * @param injector The Angular injector for dependency injection.
   */
  constructor(injector: Injector) {
    super(injector);
  }

  /**
   * Initialize SignalR communication for both list refresh and item updates.
   * @param crudItemService The airport CRUD service.
   */
  initialize(crudItemService: CrudItemService<Airport>) {
    // Call parent to setup list refresh listener
    super.initialize(crudItemService);
  }

  /**
   * Register a callback to be notified when an airport is updated.
   * Used by edit components to detect if the current item has been modified externally.
   * @param callback Function to call when an airport update is received.
   */
  registerUpdate(callback: (args: any) => void) {
    console.log(
      '%c [Airports] Register SignalR : update-airport',
      'color: purple; font-weight: bold'
    );
    this.signalRService.addMethod('update-airport', args => {
      callback(args);
    });
  }

  /**
   * Cleanup SignalR communication.
   * @param crudItemService The airport CRUD service.
   */
  destroy(crudItemService: CrudItemService<Airport>) {
    this.unregisterUpdate();
    // Call parent to cleanup list refresh listener
    super.destroy(crudItemService);
  }

  /**
   * Unregister the item update listener.
   */
  private unregisterUpdate() {
    console.log(
      '%c [Airports] Unregister SignalR : update-airport',
      'color: purple; font-weight: bold'
    );
    this.signalRService.removeMethod('update-airport');
  }
}
