import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { map, Observable } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemSignalRService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-signalr.service';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { Plane } from '../model/plane';
import { planeCRUDConfiguration } from '../plane.constants';
import { FeaturePlanesStore } from '../store/plane.state';
import { FeaturePlanesActions } from '../store/planes-actions';
import { PlaneDas } from './plane-das.service';
import { PlaneOptionsService } from './plane-options.service';

@Injectable({
  providedIn: 'root',
})
export class PlaneService extends CrudItemService<Plane> {
  _updateSuccessActionType = FeaturePlanesActions.loadAllByPost.type;
  _createSuccessActionType = FeaturePlanesActions.loadAllByPost.type;
  _updateFailureActionType = FeaturePlanesActions.failure.type;

  constructor(
    private store: Store<AppState>,
    public dasService: PlaneDas,
    public signalRService: CrudItemSignalRService<Plane>,
    protected authService: AuthService,
    public optionsService: PlaneOptionsService,
    protected injector: Injector
  ) {
    super(dasService, signalRService, optionsService, injector);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Plane : adapt the parent Key to the context. It can be null if root crud
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

  public crudItem$: Observable<Plane> = this.store.select(
    FeaturePlanesStore.getCurrentPlane
  );

  public displayItemName$: Observable<string> = this.crudItem$.pipe(
    map(plane => plane?.msn?.toString() ?? '')
  );

  public loadingGet$: Observable<boolean> = this.store.select(
    FeaturePlanesStore.getPlaneLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(FeaturePlanesActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(FeaturePlanesActions.loadAllByPost({ event }));
  }
  public create(crudItem: Plane) {
    crudItem.siteId = this.getParentIds()[0];
    this.store.dispatch(FeaturePlanesActions.create({ plane: crudItem }));
  }
  public save(crudItems: Plane[]) {
    crudItems.map(x => (x.siteId = this.getParentIds()[0]));
    this.store.dispatch(FeaturePlanesActions.save({ planes: crudItems }));
  }
  public update(crudItem: Plane) {
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
    this._currentCrudItem = <Plane>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeaturePlanesActions.clearCurrent());
  }
  // Begin BIADemo
  public updateFixedStatus(id: any, isFixed: boolean): void {
    this.store.dispatch(
      FeaturePlanesActions.updateFixedStatus({ id: id, isFixed: isFixed })
    );
  }
  // End BIADemo
}
