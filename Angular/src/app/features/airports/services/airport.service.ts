import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { AppState } from 'src/app/store/state';
import { Airport } from '../model/airport';
import { AirportCRUDConfiguration } from '../airport.constants';
import { FeatureAirportsStore } from '../store/airport.state';
import { FeatureAirportsActions } from '../store/airports-actions';
import { AirportOptionsService } from './airport-options.service';
import { AirportDas } from './airport-das.service';
import { CrudItemSignalRService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-signalr.service';

@Injectable({
    providedIn: 'root'
})
export class AirportService extends CrudItemService<Airport> {

    constructor(private store: Store<AppState>,
        public dasService: AirportDas,
        public signalRService: CrudItemSignalRService<Airport>,
        public optionsService: AirportOptionsService,
        // requiered only for parent key
        protected authService: AuthService,
        ) {
        super(dasService,signalRService,optionsService);
    }

    public getParentIds(): any[]
    {
        // TODO after creation of CRUD Airport : adapt the parent Key tothe context. It can be null if root crud
        return [];
    }

    public getFeatureName()  {  return AirportCRUDConfiguration.featureName; };

    public crudItems$: Observable<Airport[]> = this.store.select(FeatureAirportsStore.getAllAirports);
    public totalCount$: Observable<number> = this.store.select(FeatureAirportsStore.getAirportsTotalCount);
    public loadingGetAll$: Observable<boolean> = this.store.select(FeatureAirportsStore.getAirportLoadingGetAll);;
    public lastLazyLoadEvent$: Observable<LazyLoadEvent> = this.store.select(FeatureAirportsStore.getLastLazyLoadEvent);

    public crudItem$: Observable<Airport> = this.store.select(FeatureAirportsStore.getCurrentAirport);
    public loadingGet$: Observable<boolean> = this.store.select(FeatureAirportsStore.getAirportLoadingGet);

    public load(id: any){
        this.store.dispatch(FeatureAirportsActions.load({ id }));
    }
    public loadAllByPost(event: LazyLoadEvent){
        this.store.dispatch(FeatureAirportsActions.loadAllByPost({ event }));
    }
    public create(crudItem: Airport){
        // TODO after creation of CRUD Airport : map parent Key on the corresponding field
        // crudItem.siteId = this.getParentIds()[0],
        this.store.dispatch(FeatureAirportsActions.create({ airport : crudItem }));
    }
    public update(crudItem: Airport){
        this.store.dispatch(FeatureAirportsActions.update({ airport : crudItem }));
    }
    public remove(id: any){
        this.store.dispatch(FeatureAirportsActions.remove({ id }));
    }
    public multiRemove(ids: any[]){
        this.store.dispatch(FeatureAirportsActions.multiRemove({ ids }));
    }
}
