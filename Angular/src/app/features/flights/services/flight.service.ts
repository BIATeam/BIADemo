import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { AuthService } from 'packages/bia-ng/core/public-api';
import {
  CrudItemService,
  CrudItemSignalRService,
} from 'packages/bia-ng/shared/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { map, Observable } from 'rxjs';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { flightCRUDConfiguration } from '../flight.constants';
import { Flight } from '../model/flight';
import { FeatureFlightsStore } from '../store/flight.state';
import { FeatureFlightsActions } from '../store/flights-actions';
import { FlightDas } from './flight-das.service';
import { FlightOptionsService } from './flight-options.service';

@Injectable({
  providedIn: 'root',
})
export class FlightService extends CrudItemService<Flight> {
  _updateSuccessActionType = FeatureFlightsActions.loadAllByPost.type;
  _createSuccessActionType = FeatureFlightsActions.loadAllByPost.type;
  _updateFailureActionType = FeatureFlightsActions.failure.type;

  constructor(
    private store: Store<AppState>,
    public dasService: FlightDas,
    public signalRService: CrudItemSignalRService<Flight>,
    protected authService: AuthService,
    public optionsService: FlightOptionsService,
    protected injector: Injector
  ) {
    super(dasService, signalRService, optionsService, injector);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Flight : adapt the parent Key to the context. It can be null if root crud
    return [this.authService.getCurrentTeamId(TeamTypeId.Site)];
  }

  public getFeatureName() {
    return flightCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<Flight[]> = this.store.select(
    FeatureFlightsStore.getAllFlights
  );
  public totalCount$: Observable<number> = this.store.select(
    FeatureFlightsStore.getFlightsTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    FeatureFlightsStore.getFlightLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    FeatureFlightsStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<Flight> = this.store.select(
    FeatureFlightsStore.getCurrentFlight
  );

  public displayItemName$: Observable<string> = this.crudItem$.pipe(
    map(flight => flight?.id?.toString() ?? '')
  );

  public loadingGet$: Observable<boolean> = this.store.select(
    FeatureFlightsStore.getFlightLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(FeatureFlightsActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(FeatureFlightsActions.loadAllByPost({ event }));
  }
  public create(crudItem: Flight) {
    crudItem.siteId = this.getParentIds()[0];
    this.store.dispatch(FeatureFlightsActions.create({ flight: crudItem }));
  }
  public save(crudItems: Flight[]) {
    crudItems.map(x => (x.siteId = this.getParentIds()[0]));
    this.store.dispatch(FeatureFlightsActions.save({ flights: crudItems }));
  }
  public update(crudItem: Flight) {
    this.store.dispatch(FeatureFlightsActions.update({ flight: crudItem }));
  }
  public remove(id: any) {
    this.store.dispatch(FeatureFlightsActions.remove({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(FeatureFlightsActions.multiRemove({ ids }));
  }
  public clearAll() {
    this.store.dispatch(FeatureFlightsActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <Flight>{ id: '' };
    this._currentCrudItemId = '';
    this.store.dispatch(FeatureFlightsActions.clearCurrent());
  }
  public updateFixedStatus(id: any, isFixed: boolean): void {
    this.store.dispatch(
      FeatureFlightsActions.updateFixedStatus({ id: id, isFixed: isFixed })
    );
  }
}
