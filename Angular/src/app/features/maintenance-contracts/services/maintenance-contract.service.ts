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
import { maintenanceContractCRUDConfiguration } from '../maintenance-contract.constants';
import { MaintenanceContract } from '../model/maintenance-contract';
import { FeatureMaintenanceContractsStore } from '../store/maintenance-contract.state';
import { FeatureMaintenanceContractsActions } from '../store/maintenance-contracts-actions';
import { MaintenanceContractDas } from './maintenance-contract-das.service';
import { MaintenanceContractOptionsService } from './maintenance-contract-options.service';

@Injectable({
  providedIn: 'root',
})
export class MaintenanceContractService extends CrudItemService<MaintenanceContract> {
  constructor(
    private store: Store<AppState>,
    public dasService: MaintenanceContractDas,
    public signalRService: CrudItemSignalRService<MaintenanceContract>,
    public optionsService: MaintenanceContractOptionsService,
    protected injector: Injector,
    // required only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService, injector);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD MaintenanceContract : adapt the parent Key tothe context. It can be null if root crud
    return [
      this.authService.getCurrentTeamId(TeamTypeId.Site),
      this.authService.getCurrentTeamId(TeamTypeId.AircraftMaintenanceCompany),
    ];
  }

  public getFeatureName() {
    return maintenanceContractCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<MaintenanceContract[]> = this.store.select(
    FeatureMaintenanceContractsStore.getAllMaintenanceContracts
  );
  public totalCount$: Observable<number> = this.store.select(
    FeatureMaintenanceContractsStore.getMaintenanceContractsTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    FeatureMaintenanceContractsStore.getMaintenanceContractLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    FeatureMaintenanceContractsStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<MaintenanceContract> = this.store.select(
    FeatureMaintenanceContractsStore.getCurrentMaintenanceContract
  );

  public displayItemName$: Observable<string> = this.crudItem$.pipe(
    map(
      maintenanceContract =>
        maintenanceContract?.contractNumber?.toString() ?? ''
    )
  );

  public loadingGet$: Observable<boolean> = this.store.select(
    FeatureMaintenanceContractsStore.getMaintenanceContractLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(FeatureMaintenanceContractsActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(
      FeatureMaintenanceContractsActions.loadAllByPost({ event })
    );
  }
  public create(crudItem: MaintenanceContract) {
    // TODO after creation of CRUD MaintenanceContract : map parent Key on the corresponding field
    this.store.dispatch(
      FeatureMaintenanceContractsActions.create({
        maintenanceContract: crudItem,
      })
    );
  }
  public update(crudItem: MaintenanceContract) {
    crudItem.siteId = this.getParentIds()[0];
    crudItem.aircraftMaintenanceCompanyId = this.getParentIds()[1];
    this.store.dispatch(
      FeatureMaintenanceContractsActions.update({
        maintenanceContract: crudItem,
      })
    );
  }
  public save(crudItems: MaintenanceContract[]) {
    crudItems.map(x => {
      x.siteId = this.getParentIds()[0];
      x.aircraftMaintenanceCompanyId = this.getParentIds()[1];
    });
    this.store.dispatch(
      FeatureMaintenanceContractsActions.save({
        maintenanceContracts: crudItems,
      })
    );
  }
  public remove(id: any) {
    this.store.dispatch(FeatureMaintenanceContractsActions.remove({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(
      FeatureMaintenanceContractsActions.multiRemove({ ids })
    );
  }
  public clearAll() {
    this.store.dispatch(FeatureMaintenanceContractsActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <MaintenanceContract>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeatureMaintenanceContractsActions.clearCurrent());
  }
}
