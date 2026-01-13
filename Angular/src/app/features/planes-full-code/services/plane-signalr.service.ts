import { Injectable } from '@angular/core';
import { AuthService, BiaSignalRService } from '@bia-team/bia-ng/core';
import { TargetedFeature } from '@bia-team/bia-ng/models';
import { Store } from '@ngrx/store';
import { first } from 'rxjs/operators';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { getLastLazyLoadEvent } from '../store/plane.state';
import { FeaturePlanesActions } from '../store/planes-actions';

/**
 * Service managing SignalR events for hangfire jobs.
 * To use it:
 * - Add a parameter of this type in the constructor of your component (for dependency injection)
 * - Call the 'initialize()' method on it, so that dependency injection is truly performed
 */
@Injectable({
  providedIn: 'root',
})
export class PlanesSignalRService {
  private targetedFeature: TargetedFeature;

  /**
   * Constructor.
   * @param store the store.
   * @param signalRService the service managing the SignalR connection.
   */
  constructor(
    private store: Store<AppState>,
    private signalRService: BiaSignalRService,
    private authService: AuthService
  ) {}

  /**
   * Initialize SignalR communication.
   * Note: this method has been created so that we have to call one method on this class, otherwise dependency injection is not working.
   */
  initialize() {
    console.log(
      '%c [Planes] Register SignalR : refresh-planes',
      'color: purple; font-weight: bold'
    );
    this.signalRService.addMethod('refresh-planes', () => {
      this.store
        .select(getLastLazyLoadEvent)
        .pipe(first())
        .subscribe(event => {
          console.log(
            '%c [Planes] RefreshSuccess',
            'color: green; font-weight: bold'
          );
          this.store.dispatch(
            FeaturePlanesActions.loadAllByPost({
              event: event,
            })
          );
        });
    });
    this.targetedFeature = {
      parentKey: this.authService.getCurrentTeamId(TeamTypeId.Site).toString(),
      featureName: 'planes',
    };
    this.signalRService.joinGroup(this.targetedFeature);
  }

  destroy() {
    console.log(
      '%c [Planes] Unregister SignalR : refresh-planes',
      'color: purple; font-weight: bold'
    );
    this.signalRService.removeMethod('refresh-planes');
    this.signalRService.leaveGroup(this.targetedFeature);
  }
}
