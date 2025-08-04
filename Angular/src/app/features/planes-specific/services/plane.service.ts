import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { AuthService, clone } from 'bia-ng/core';
import { CrudItemService, CrudItemSignalRService } from 'bia-ng/shared';
import { TableLazyLoadEvent } from 'primeng/table';
import { map, Observable } from 'rxjs';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { Plane } from '../model/plane';
import { PlaneSpecific } from '../model/plane-specific';
import { planeCRUDConfiguration } from '../plane.constants';
import { FeaturePlanesStore } from '../store/plane.state';
import { FeaturePlanesActions } from '../store/planes-actions';
import { PlaneDas } from './plane-das.service';
import { PlaneOptionsService } from './plane-options.service';

@Injectable({
  providedIn: 'root',
})
export class PlaneService extends CrudItemService<Plane, PlaneSpecific> {
  _updateSuccessActionType = FeaturePlanesActions.loadAllByPost.type;
  _createSuccessActionType = FeaturePlanesActions.loadAllByPost.type;
  _updateFailureActionType = FeaturePlanesActions.failure.type;

  constructor(
    private store: Store<AppState>,
    public dasService: PlaneDas,
    public signalRService: CrudItemSignalRService<Plane>,
    public optionsService: PlaneOptionsService,
    protected injector: Injector,
    // required only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService, injector);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Plane : adapt the parent Key tothe context. It can be null if root crud
    return [this.authService.getCurrentTeamId(TeamTypeId.Site)];
  }

  public getFeatureName() {
    return planeCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<Plane[]> = this.store.select(
    FeaturePlanesStore.getAllPlanes
  );

  public totalCount$: Observable<number> = this.store.select(
    FeaturePlanesStore.getPlanesTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    FeaturePlanesStore.getPlaneLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    FeaturePlanesStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<PlaneSpecific> = this.store
    .select(FeaturePlanesStore.getCurrentPlane)
    .pipe(map(plane => clone(plane)));

  public loadingGet$: Observable<boolean> = this.store.select(
    FeaturePlanesStore.getPlaneLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(FeaturePlanesActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(FeaturePlanesActions.loadAllByPost({ event }));
  }
  public create(crudItem: PlaneSpecific) {
    // TODO after creation of CRUD Plane : map parent Key on the corresponding field
    this.resetNewItemsIds(crudItem.engines);
    crudItem.siteId = this.getParentIds()[0];
    this.store.dispatch(FeaturePlanesActions.create({ plane: crudItem }));
  }
  public update(crudItem: PlaneSpecific) {
    this.resetNewItemsIds(crudItem.engines);
    this.store.dispatch(FeaturePlanesActions.update({ plane: crudItem }));
  }
  public remove(id: any) {
    this.store.dispatch(FeaturePlanesActions.remove({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(FeaturePlanesActions.multiRemove({ ids }));
  }
  public clearAll() {
    this.store.dispatch(FeaturePlanesActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <PlaneSpecific>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeaturePlanesActions.clearCurrent());
  }
}
