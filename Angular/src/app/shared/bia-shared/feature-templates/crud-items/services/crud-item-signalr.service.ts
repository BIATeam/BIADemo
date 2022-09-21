import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { first } from 'rxjs/operators';
import { BiaSignalRService } from 'src/app/core/bia-core/services/bia-signalr.service';
import { TargetedFeature } from 'src/app/shared/bia-shared/model/signalR';
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
  private targetedFeature: TargetedFeature;

  /**
   * Constructor.
   * @param store the store.
   * @param signalRService the service managing the SignalR connection.
   */
  protected signalRService: BiaSignalRService;
  protected store: Store<AppState>;
  
  constructor(
    protected injector: Injector
  ) {
    this.store = this.injector.get<Store<AppState>>(Store);
    this.signalRService = this.injector.get<BiaSignalRService>(BiaSignalRService);
   }

  /**
   * Initialize SignalR communication.
   * Note: this method has been created so that we have to call one method on this class, otherwise dependency injection is not working.
   */
  initialize(crudItemService: CrudItemService<CrudItem>, ) {
    this.targetedFeature = crudItemService.getSignalRTargetedFeature();

    console.log('%c [' + crudItemService.getConsoleLabel() + '] Register SignalR : ' + crudItemService.getSignalRRefreshEvent(), 'color: purple; font-weight: bold');
    this.signalRService.addMethod(crudItemService.getSignalRRefreshEvent(), () => {
      crudItemService.lastLazyLoadEvent$.pipe(first()).subscribe(
        (event) => {
          console.log('%c [' + crudItemService.getConsoleLabel() + '] RefreshSuccess', 'color: green; font-weight: bold');
          crudItemService.loadAllByPost(event);
        }
      );
    });
    this.signalRService.joinGroup(this.targetedFeature);
  }

  destroy() {
    console.log('%c [CrudItems] Unregister SignalR : refresh-crud-items', 'color: purple; font-weight: bold');
    this.signalRService.removeMethod('refresh-crud-items');
    this.signalRService.leaveGroup(this.targetedFeature);
  }
}
