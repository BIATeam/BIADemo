import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { CrudItemService, CrudItemSignalRService } from 'biang/shared';
import { TableLazyLoadEvent } from 'primeng/table';
import { map, Observable } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { PlaneService } from '../../../services/plane.service';
import { engineCRUDConfiguration } from '../engine.constants';
import { Engine } from '../model/engine';
import { FeatureEnginesStore } from '../store/engine.state';
import { FeatureEnginesActions } from '../store/engines-actions';
import { EngineDas } from './engine-das.service';
import { EngineOptionsService } from './engine-options.service';

@Injectable({
  providedIn: 'root',
})
export class EngineService extends CrudItemService<Engine> {
  _updateSuccessActionType = FeatureEnginesActions.loadAllByPost.type;
  _createSuccessActionType = FeatureEnginesActions.loadAllByPost.type;
  _updateFailureActionType = FeatureEnginesActions.failure.type;

  constructor(
    private store: Store<AppState>,
    public dasService: EngineDas,
    public signalRService: CrudItemSignalRService<Engine>,
    public planeService: PlaneService,
    public optionsService: EngineOptionsService,
    protected injector: Injector
  ) {
    super(dasService, signalRService, optionsService, injector);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Engine : adapt the parent Key to the context. It can be null if root crud
    // For child : set the Id of the Parent
    return [this.planeService.currentCrudItemId];
  }

  public getFeatureName() {
    return engineCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<Engine[]> = this.store.select(
    FeatureEnginesStore.getAllEngines
  );
  public totalCount$: Observable<number> = this.store.select(
    FeatureEnginesStore.getEnginesTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    FeatureEnginesStore.getEngineLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    FeatureEnginesStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<Engine> = this.store.select(
    FeatureEnginesStore.getCurrentEngine
  );

  public displayItemName$: Observable<string> = this.crudItem$.pipe(
    map(engine => engine?.reference?.toString() ?? '')
  );

  public loadingGet$: Observable<boolean> = this.store.select(
    FeatureEnginesStore.getEngineLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(FeatureEnginesActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(FeatureEnginesActions.loadAllByPost({ event }));
  }
  public create(crudItem: Engine) {
    // TODO after creation of CRUD Engine : map parent Key on the corresponding field
    crudItem.planeId = this.getParentIds()[0];
    this.store.dispatch(FeatureEnginesActions.create({ engine: crudItem }));
  }
  public save(crudItems: Engine[]) {
    // TODO after creation of CRUD Engine : map parent Key on the corresponding field
    crudItems.map(x => (x.planeId = this.getParentIds()[0]));
    this.store.dispatch(FeatureEnginesActions.save({ engines: crudItems }));
  }
  public update(crudItem: Engine) {
    this.store.dispatch(FeatureEnginesActions.update({ engine: crudItem }));
  }
  public remove(id: any) {
    this.store.dispatch(FeatureEnginesActions.remove({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(FeatureEnginesActions.multiRemove({ ids }));
  }
  public clearAll() {
    this.store.dispatch(FeatureEnginesActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <Engine>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeatureEnginesActions.clearCurrent());
  }
  public updateFixedStatus(id: any, isFixed: boolean): void {
    this.store.dispatch(
      FeatureEnginesActions.updateFixedStatus({ id: id, isFixed: isFixed })
    );
  }
}
