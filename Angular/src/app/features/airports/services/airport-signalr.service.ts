import { Injectable } from '@angular/core';
import { BiaSignalRService } from 'bia-ng/core';
import { TargetedFeature } from 'bia-ng/models';

/**
 * Service managing SignalR events for hangfire jobs.
 * To use it:
 * - Add a parameter of this type in the constructor of your component (for dependency injection)
 * - Call the 'initialize()' method on it, so that dependency injection is truly performed
 */
@Injectable({
  providedIn: 'root',
})
export class AirportSignalRService {
  private targetedFeature: TargetedFeature;

  /**
   * Constructor.
   * @param store the store.
   * @param signalRService the service managing the SignalR connection.
   */
  constructor(private signalRService: BiaSignalRService) {}

  /**
   * Initialize SignalR communication.
   * Note: this method has been created so that we have to call one method on this class, otherwise dependency injection is not working.
   */
  initialize() {
    this.targetedFeature = {
      parentKey: '',
      featureName: 'airports',
    };
    this.signalRService.joinGroup(this.targetedFeature);
  }

  registerUpdate(callback: (args: any) => void) {
    console.log(
      '%c [Planes] Register SignalR : update-airport',
      'color: purple; font-weight: bold'
    );
    this.signalRService.addMethod('update-airport', args => {
      callback(args);
    });
  }

  destroy() {
    this.unregisterUpdate();
    this.signalRService.leaveGroup(this.targetedFeature);
  }

  private unregisterUpdate() {
    console.log(
      '%c [Planes] Unregister SignalR : update-airport',
      'color: purple; font-weight: bold'
    );
    this.signalRService.removeMethod('update-airport');
  }
}
