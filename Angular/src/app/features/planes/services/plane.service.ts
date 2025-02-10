import { Injectable } from '@angular/core';
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
  constructor(
    private store: Store<AppState>,
    public dasService: PlaneDas,
    public signalRService: CrudItemSignalRService<Plane>,
    public optionsService: PlaneOptionsService,
    // required only for parent key
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

  public crudItem$: Observable<Plane> = this.store.select(
    FeaturePlanesStore.getCurrentPlane
  );

  public displayItemName$: Observable<string> = this.crudItem$.pipe(
    /// BIAToolKit - Begin Display msn
    map(plane => plane?.msn?.toString() ?? '')
    /// BIAToolKit - End Display msn
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
    // TODO after creation of CRUD Plane : map parent Key on the corresponding field
    /// BIAToolKit - Begin Parent
    let indexParent = 0;
    /// BIAToolKit - End Parent
    /// BIAToolKit - Begin Parent siteId
    crudItem.siteId = this.getParentIds()[indexParent++];
    /// BIAToolKit - End Parent siteId
    this.store.dispatch(FeaturePlanesActions.create({ plane: crudItem }));
  }
  public update(crudItem: Plane) {
    this.store.dispatch(FeaturePlanesActions.update({ plane: crudItem }));
  }
  public save(crudItems: Plane[]) {
    /// BIAToolKit - Begin Parent
    let indexParent = 0;
    /// BIAToolKit - End Parent
    /// BIAToolKit - Begin Parent siteId
    const siteIdIndexParent = indexParent++;
    crudItems
      .filter(x => !x.id)
      .map(x => (x.siteId = this.getParentIds()[siteIdIndexParent]));
    /// BIAToolKit - End Parent siteId
    this.store.dispatch(FeaturePlanesActions.save({ planes: crudItems }));
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
}
