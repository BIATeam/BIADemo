import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { first } from 'rxjs/operators';
import { BiaSignalRService } from 'src/app/core/bia-core/services/bia-signalr.service';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { TargetedFeature } from 'src/app/shared/bia-shared/model/signalR';
import { TeamTypeId } from 'src/app/shared/constants';
import { CrudItemService } from './crud-item.service';
import { BaseDto } from '../../../model/base-dto';

/**
 * Service managing SignalR events for hangfire jobs.
 * To use it:
 * - Add a parameter of this type in the constructor of your component (for dependency injection)
 * - Call the 'initialize()' method on it, so that dependency injection is truly performed
 */
@Injectable({
    providedIn: 'root'
})
export class CrudItemSignalRService<CrudItem extends BaseDto> {
  protected targetedFeature: TargetedFeature;

  /**
   * Constructor.
   * @param store the store.
   * @param signalRService the service managing the SignalR connection.
   */
  constructor(protected store: Store<AppState>, 
    protected signalRService: BiaSignalRService, 
    protected authService: AuthService,
    ) {
  }

  /**
   * Initialize SignalR communication.
   * Note: this method has been created so that we have to call one method on this class, otherwise dependency injection is not working.
   */
  initialize(crudItemService : CrudItemService<CrudItem>) {
    console.log('%c [CrudItems] Register SignalR : refresh-crud-items', 'color: purple; font-weight: bold');
    this.signalRService.addMethod('refresh-crud-items', () => {
      crudItemService.lastLazyLoadEvent$.pipe(first()).subscribe(
        (event) => {
          console.log('%c [CrudItems] RefreshSuccess', 'color: green; font-weight: bold');
          crudItemService.loadAllByPost(event);
        }
      );
    });
    this.targetedFeature = {parentKey: this.authService.getCurrentTeamId(TeamTypeId.Site).toString() , featureName : 'crud-items'};
    this.signalRService.joinGroup(this.targetedFeature);
  }

  destroy() {
    console.log('%c [CrudItems] Unregister SignalR : refresh-crud-items', 'color: purple; font-weight: bold');
    this.signalRService.removeMethod('refresh-crud-items');
    this.signalRService.leaveGroup(this.targetedFeature);
  }
}
