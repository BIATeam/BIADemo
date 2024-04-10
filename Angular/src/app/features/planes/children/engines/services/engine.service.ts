import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Observable } from 'rxjs';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { AppState } from 'src/app/store/state';
import { Engine } from '../model/engine';
import { EngineCRUDConfiguration } from '../engine.constants';
import { FeatureEnginesStore } from '../store/engine.state';
import { FeatureEnginesActions } from '../store/engines-actions';
import { EngineOptionsService } from './engine-options.service';
import { EngineDas } from './engine-das.service';
import { CrudItemSignalRService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-signalr.service';
import { PlaneService } from '../../../services/plane.service';

@Injectable({
    providedIn: 'root'
})
export class EngineService extends CrudItemService<Engine> {

    constructor(private store: Store<AppState>,
        public dasService: EngineDas,
        public signalRService: CrudItemSignalRService<Engine>,
        public optionsService: EngineOptionsService,
        // requiered only for parent key
        protected planeService: PlaneService,
        ) {
        super(dasService,signalRService,optionsService);
    }

    public getParentIds(): any[] {
        // TODO after creation of CRUD Engine : adapt the parent Key to the context. It can be null if root crud
		// For child : set the Id of the Parent
        return  [this.planeService.currentCrudItemId];
    }

    public getFeatureName()  {  return EngineCRUDConfiguration.featureName; };

    public crudItems$: Observable<Engine[]> = this.store.select(FeatureEnginesStore.getAllEngines);
    public totalCount$: Observable<number> = this.store.select(FeatureEnginesStore.getEnginesTotalCount);
    public loadingGetAll$: Observable<boolean> = this.store.select(FeatureEnginesStore.getEngineLoadingGetAll);;
    public lastLazyLoadEvent$: Observable<LazyLoadEvent> = this.store.select(FeatureEnginesStore.getLastLazyLoadEvent);

    public crudItem$: Observable<Engine> = this.store.select(FeatureEnginesStore.getCurrentEngine);
    public loadingGet$: Observable<boolean> = this.store.select(FeatureEnginesStore.getEngineLoadingGet);

    public load(id: any){
        this.store.dispatch(FeatureEnginesActions.load({ id }));
    }
    public loadAllByPost(event: LazyLoadEvent){
        this.store.dispatch(FeatureEnginesActions.loadAllByPost({ event }));
    }
    public create(crudItem: Engine){
        // TODO after creation of CRUD Engine : map parent Key on the corresponding field
        crudItem.planeId = this.getParentIds()[0],
        this.store.dispatch(FeatureEnginesActions.create({ engine : crudItem }));
    }
    public update(crudItem: Engine){
        this.store.dispatch(FeatureEnginesActions.update({ engine : crudItem }));
    }
    public remove(id: any){
        this.store.dispatch(FeatureEnginesActions.remove({ id }));
    }
    public multiRemove(ids: any[]){
        this.store.dispatch(FeatureEnginesActions.multiRemove({ ids }));
    }
    public clearAll(){
        this.store.dispatch(FeatureEnginesActions.clearAll());
    }
    public clearCurrent(){
        this._currentCrudItem = <Engine>{};
        this._currentCrudItemId = 0;
        this.store.dispatch(FeatureEnginesActions.clearCurrent());
    }
}
