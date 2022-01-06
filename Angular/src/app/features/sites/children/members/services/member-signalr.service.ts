import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { first } from 'rxjs/operators';
import { BiaSignalRService } from 'src/app/core/bia-core/services/bia-signalr.service';
import { loadAllByPost } from '../store/members-actions';
import { getLastLazyLoadEvent } from '../store/member.state';
import { LazyLoadEvent } from 'primeng/api';
import { SiteService } from '../../../services/site.service';
import { TargetedFeature } from 'src/app/shared/bia-shared/model/signalR';

/**
 * Service managing SignalR events for hangfire jobs.
 * To use it:
 * - Add a parameter of this type in the constructor of your component (for dependency injection)
 * - Call the 'initialize()' method on it, so that dependency injection is truly performed
 */
@Injectable({
    providedIn: 'root'
})
export class MembersSignalRService {

  private targetedFeature: TargetedFeature;

  /**
   * Constructor.
   * @param store the store.
   * @param signalRService the service managing the SignalR connection.
   */
  constructor(private store: Store<AppState>, private signalRService: BiaSignalRService, private siteService: SiteService) {
    // Do nothing.
  }

  /**
   * Initialize SignalR communication.
   * Note: this method has been created so that we have to call one method on this class, otherwise dependency injection is not working.
   */
  initialize() {
    console.log('%c [Members] Register SignalR : refresh-members', 'color: purple; font-weight: bold');
    this.signalRService.addMethod('refresh-members', () => {
      this.store.select(getLastLazyLoadEvent).pipe(first()).subscribe(
        (event) => {
          console.log('%c [Members] RefreshSuccess', 'color: green; font-weight: bold');
          this.store.dispatch(loadAllByPost({ event: <LazyLoadEvent>event }));
        }
      );
    });
    this.targetedFeature = {parentKey: this.siteService.currentSiteId.toString() , featureName: 'members'};
    this.signalRService.joinGroup(this.targetedFeature);

  }

  destroy() {
    console.log('%c [Members] Unregister SignalR : refresh-members', 'color: purple; font-weight: bold');
    this.signalRService.removeMethod('refresh-members');
    this.signalRService.leaveGroup(this.targetedFeature);
  }
}
