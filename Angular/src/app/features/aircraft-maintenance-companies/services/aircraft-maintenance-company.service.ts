import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemSignalRService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-signalr.service';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { aircraftMaintenanceCompanyCRUDConfiguration } from '../aircraft-maintenance-company.constants';
import { AircraftMaintenanceCompany } from '../model/aircraft-maintenance-company';
import { FeatureAircraftMaintenanceCompaniesActions } from '../store/aircraft-maintenance-companies-actions';
import { FeatureAircraftMaintenanceCompaniesStore } from '../store/aircraft-maintenance-company.state';
import { AircraftMaintenanceCompanyDas } from './aircraft-maintenance-company-das.service';
import { AircraftMaintenanceCompanyOptionsService } from './aircraft-maintenance-company-options.service';

@Injectable({
  providedIn: 'root',
})
export class AircraftMaintenanceCompanyService extends CrudItemService<AircraftMaintenanceCompany> {
  _updateSuccessActionType =
    FeatureAircraftMaintenanceCompaniesActions.loadAllByPost.type;
  _createSuccessActionType =
    FeatureAircraftMaintenanceCompaniesActions.loadAllByPost.type;
  _updateFailureActionType =
    FeatureAircraftMaintenanceCompaniesActions.failure.type;

  constructor(
    private store: Store<AppState>,
    public dasService: AircraftMaintenanceCompanyDas,
    public signalRService: CrudItemSignalRService<AircraftMaintenanceCompany>,
    public optionsService: AircraftMaintenanceCompanyOptionsService,
    protected injector: Injector,
    // required only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService, injector);
  }

  // Customization for teams
  public get currentCrudItemId(): any {
    // should be redefine due to the setter
    return super.currentCrudItemId;
  }

  // Customization for teams
  public set currentCrudItemId(id: any) {
    if (this._currentCrudItemId !== id) {
      this._currentCrudItemId = id;
      this.authService.changeCurrentTeamId(
        TeamTypeId.AircraftMaintenanceCompany,
        id
      );
    }
    this.load(id);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the parent Key tothe context. It can be null if root crud
    return [];
  }

  public getFeatureName() {
    return aircraftMaintenanceCompanyCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<AircraftMaintenanceCompany[]> =
    this.store.select(
      FeatureAircraftMaintenanceCompaniesStore.getAllAircraftMaintenanceCompanies
    );
  public totalCount$: Observable<number> = this.store.select(
    FeatureAircraftMaintenanceCompaniesStore.getAircraftMaintenanceCompaniesTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    FeatureAircraftMaintenanceCompaniesStore.getAircraftMaintenanceCompanyLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    FeatureAircraftMaintenanceCompaniesStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<AircraftMaintenanceCompany> = this.store.select(
    FeatureAircraftMaintenanceCompaniesStore.getCurrentAircraftMaintenanceCompany
  );
  public loadingGet$: Observable<boolean> = this.store.select(
    FeatureAircraftMaintenanceCompaniesStore.getAircraftMaintenanceCompanyLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(
      FeatureAircraftMaintenanceCompaniesActions.load({ id })
    );
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(
      FeatureAircraftMaintenanceCompaniesActions.loadAllByPost({ event })
    );
  }
  public create(crudItem: AircraftMaintenanceCompany) {
    // TODO after creation of CRUD Team AircraftMaintenanceCompany : map parent Key on the corresponding field
    // crudItem.siteId = this.getParentIds()[0],
    this.store.dispatch(
      FeatureAircraftMaintenanceCompaniesActions.create({
        aircraftMaintenanceCompany: crudItem,
      })
    );
  }
  public update(crudItem: AircraftMaintenanceCompany) {
    this.store.dispatch(
      FeatureAircraftMaintenanceCompaniesActions.update({
        aircraftMaintenanceCompany: crudItem,
      })
    );
  }
  public remove(id: any) {
    this.store.dispatch(
      FeatureAircraftMaintenanceCompaniesActions.remove({ id })
    );
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(
      FeatureAircraftMaintenanceCompaniesActions.multiRemove({ ids })
    );
  }
  public clearAll() {
    this.store.dispatch(FeatureAircraftMaintenanceCompaniesActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <AircraftMaintenanceCompany>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(
      FeatureAircraftMaintenanceCompaniesActions.clearCurrent()
    );
  }
}
