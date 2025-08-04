import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { AuthService } from 'bia-ng/core';
import { CrudItemService, CrudItemSignalRService } from 'bia-ng/shared';
import { TableLazyLoadEvent } from 'primeng/table';
import { Observable } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { airportCRUDConfiguration } from '../airport.constants';
import { Airport } from '../model/airport';
import { FeatureAirportsStore } from '../store/airport.state';
import { FeatureAirportsActions } from '../store/airports-actions';
import { AirportDas } from './airport-das.service';
import { AirportOptionsService } from './airport-options.service';

@Injectable({
  providedIn: 'root',
})
export class AirportService extends CrudItemService<Airport> {
  _updateSuccessActionType = FeatureAirportsActions.loadAllByPost.type;
  _createSuccessActionType = FeatureAirportsActions.loadAllByPost.type;
  _updateFailureActionType = FeatureAirportsActions.failure.type;

  constructor(
    private store: Store<AppState>,
    public dasService: AirportDas,
    public signalRService: CrudItemSignalRService<Airport>,
    public optionsService: AirportOptionsService,
    protected injector: Injector,
    // required only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService, injector);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Airport : adapt the parent Key tothe context. It can be null if root crud
    return [];
  }

  public getFeatureName() {
    return airportCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<Airport[]> = this.store.select(
    FeatureAirportsStore.getAllAirports
  );
  public totalCount$: Observable<number> = this.store.select(
    FeatureAirportsStore.getAirportsTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    FeatureAirportsStore.getAirportLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    FeatureAirportsStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<Airport> = this.store.select(
    FeatureAirportsStore.getCurrentAirport
  );
  public loadingGet$: Observable<boolean> = this.store.select(
    FeatureAirportsStore.getAirportLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(FeatureAirportsActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(FeatureAirportsActions.loadAllByPost({ event }));
  }
  public create(crudItem: Airport) {
    // TODO after creation of CRUD Airport : map parent Key on the corresponding field
    // crudItem.siteId = this.getParentIds()[0],
    this.store.dispatch(FeatureAirportsActions.create({ airport: crudItem }));
  }
  public update(crudItem: Airport) {
    this.store.dispatch(FeatureAirportsActions.update({ airport: crudItem }));
  }
  public remove(id: any) {
    this.store.dispatch(FeatureAirportsActions.remove({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(FeatureAirportsActions.multiRemove({ ids }));
  }
  public clearAll() {
    this.store.dispatch(FeatureAirportsActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <Airport>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeatureAirportsActions.clearCurrent());
  }
}
