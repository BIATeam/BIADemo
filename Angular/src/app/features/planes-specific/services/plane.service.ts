import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemSignalRService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-signalr.service';
import { CrudListAndItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-list-and-item.service';
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
export class PlaneService extends CrudListAndItemService<PlaneSpecific, Plane> {
  constructor(
    private store: Store<AppState>,
    public dasService: PlaneDas,
    public signalRService: CrudItemSignalRService<Plane>,
    public optionsService: PlaneOptionsService,
    // requiered only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService);
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

  public crudItem$: Observable<PlaneSpecific> = this.store.select(
    FeaturePlanesStore.getCurrentPlane
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
  public create(crudItem: PlaneSpecific) {
    // TODO after creation of CRUD Plane : map parent Key on the corresponding field
    this.resetNewItemsIds(crudItem.engines);
    (crudItem.siteId = this.getParentIds()[0]),
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
