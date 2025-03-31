import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { MaintenanceContract } from '../model/maintenance-contract';
import { maintenanceContractCRUDConfiguration } from '../maintenance-contract.constants';
import * as fromMaintenanceContracts from './maintenance-contracts-reducer';

export namespace FeatureMaintenanceContractsStore {
  export interface MaintenanceContractsState {
    maintenanceContracts: fromMaintenanceContracts.State;
  }

  /** Provide reducers with AoT-compilation compliance */
  export function reducers(state: MaintenanceContractsState | undefined, action: Action) {
    return combineReducers({
      maintenanceContracts: fromMaintenanceContracts.maintenanceContractReducers,
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  export const getMaintenanceContractsState = createFeatureSelector<MaintenanceContractsState>(
    maintenanceContractCRUDConfiguration.storeKey
  );

  export const getMaintenanceContractsEntitiesState = createSelector(
    getMaintenanceContractsState,
    state => state.maintenanceContracts
  );

  export const getMaintenanceContractsTotalCount = createSelector(
    getMaintenanceContractsEntitiesState,
    state => state.totalCount
  );

  export const getCurrentMaintenanceContract = createSelector(
    getMaintenanceContractsEntitiesState,
    state => state.currentItem ?? <MaintenanceContract>{}
  );

  export const getLastLazyLoadEvent = createSelector(
    getMaintenanceContractsEntitiesState,
    state => state.lastLazyLoadEvent
  );

  export const getMaintenanceContractLoadingGet = createSelector(
    getMaintenanceContractsEntitiesState,
    state => state.loadingGet
  );

  export const getMaintenanceContractLoadingGetAll = createSelector(
    getMaintenanceContractsEntitiesState,
    state => state.loadingGetAll
  );

  export const { selectAll: getAllMaintenanceContracts } =
    fromMaintenanceContracts.maintenanceContractsAdapter.getSelectors(getMaintenanceContractsEntitiesState);

  export const getMaintenanceContractById = (id: number) =>
    createSelector(getMaintenanceContractsEntitiesState, fromMaintenanceContracts.getMaintenanceContractById(id));
}
