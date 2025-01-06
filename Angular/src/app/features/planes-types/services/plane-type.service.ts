import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemSignalRService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-signalr.service';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { AppState } from 'src/app/store/state';
import { PlaneType } from '../model/plane-type';
import { planeTypeCRUDConfiguration } from '../plane-type.constants';
import { FeaturePlanesTypesStore } from '../store/plane-type.state';
import { FeaturePlanesTypesActions } from '../store/planes-types-actions';
import { PlaneTypeDas } from './plane-type-das.service';
import { PlaneTypeOptionsService } from './plane-type-options.service';

@Injectable({
  providedIn: 'root',
})
export class PlaneTypeService extends CrudItemService<PlaneType> {
  constructor(
    private store: Store<AppState>,
    public dasService: PlaneTypeDas,
    public signalRService: CrudItemSignalRService<PlaneType>,
    public optionsService: PlaneTypeOptionsService,
    // required only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD PlaneType : adapt the parent Key tothe context. It can be null if root crud
    return [];
  }

  public getFeatureName() {
    return planeTypeCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<PlaneType[]> = this.store.select(
    FeaturePlanesTypesStore.getAllPlanesTypes
  );
  public totalCount$: Observable<number> = this.store.select(
    FeaturePlanesTypesStore.getPlanesTypesTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    FeaturePlanesTypesStore.getPlaneTypeLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    FeaturePlanesTypesStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<PlaneType> = this.store.select(
    FeaturePlanesTypesStore.getCurrentPlaneType
  );
  public loadingGet$: Observable<boolean> = this.store.select(
    FeaturePlanesTypesStore.getPlaneTypeLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(FeaturePlanesTypesActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(FeaturePlanesTypesActions.loadAllByPost({ event }));
  }
  public create(crudItem: PlaneType) {
    // TODO after creation of CRUD PlaneType : map parent Key on the corresponding field
    // crudItem.siteId = this.getParentIds()[0],
    this.store.dispatch(
      FeaturePlanesTypesActions.create({ planeType: crudItem })
    );
  }
  public update(crudItem: PlaneType) {
    this.store.dispatch(
      FeaturePlanesTypesActions.update({ planeType: crudItem })
    );
  }
  public remove(id: any) {
    this.store.dispatch(FeaturePlanesTypesActions.remove({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(FeaturePlanesTypesActions.multiRemove({ ids }));
  }
  public clearAll() {
    this.store.dispatch(FeaturePlanesTypesActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <PlaneType>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeaturePlanesTypesActions.clearCurrent());
  }
}
