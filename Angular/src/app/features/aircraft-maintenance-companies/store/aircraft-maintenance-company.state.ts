import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { aircraftMaintenanceCompanyCRUDConfiguration } from '../aircraft-maintenance-company.constants';
import { AircraftMaintenanceCompany } from '../model/aircraft-maintenance-company';
import * as fromAircraftMaintenanceCompanies from './aircraft-maintenance-companies-reducer';

export namespace FeatureAircraftMaintenanceCompaniesStore {
  export interface AircraftMaintenanceCompaniesState {
    aircraftMaintenanceCompanies: fromAircraftMaintenanceCompanies.State;
  }

  /** Provide reducers with AoT-compilation compliance */
  export function reducers(
    state: AircraftMaintenanceCompaniesState | undefined,
    action: Action
  ) {
    return combineReducers({
      aircraftMaintenanceCompanies:
        fromAircraftMaintenanceCompanies.aircraftMaintenanceCompanyReducers,
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  export const getAircraftMaintenanceCompaniesState =
    createFeatureSelector<AircraftMaintenanceCompaniesState>(
      aircraftMaintenanceCompanyCRUDConfiguration.storeKey
    );

  export const getAircraftMaintenanceCompaniesEntitiesState = createSelector(
    getAircraftMaintenanceCompaniesState,
    state => state.aircraftMaintenanceCompanies
  );

  export const getAircraftMaintenanceCompaniesTotalCount = createSelector(
    getAircraftMaintenanceCompaniesEntitiesState,
    state => state.totalCount
  );

  export const getCurrentAircraftMaintenanceCompany = createSelector(
    getAircraftMaintenanceCompaniesEntitiesState,
    state => state.currentItem ?? <AircraftMaintenanceCompany>{}
  );

  export const getLastLazyLoadEvent = createSelector(
    getAircraftMaintenanceCompaniesEntitiesState,
    state => state.lastLazyLoadEvent
  );

  export const getAircraftMaintenanceCompanyLoadingGet = createSelector(
    getAircraftMaintenanceCompaniesEntitiesState,
    state => state.loadingGet
  );

  export const getAircraftMaintenanceCompanyLoadingGetAll = createSelector(
    getAircraftMaintenanceCompaniesEntitiesState,
    state => state.loadingGetAll
  );

  export const { selectAll: getAllAircraftMaintenanceCompanies } =
    fromAircraftMaintenanceCompanies.aircraftMaintenanceCompaniesAdapter.getSelectors(
      getAircraftMaintenanceCompaniesEntitiesState
    );

  export const getAircraftMaintenanceCompanyById = (id: number) =>
    createSelector(
      getAircraftMaintenanceCompaniesEntitiesState,
      fromAircraftMaintenanceCompanies.getAircraftMaintenanceCompanyById(id)
    );
}
