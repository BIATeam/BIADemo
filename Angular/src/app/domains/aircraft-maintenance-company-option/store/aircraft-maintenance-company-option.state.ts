import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { storeKey } from '../aircraft-maintenance-company-option.contants';
import * as fromAircraftMaintenanceCompanyOptions from './aircraft-maintenance-company-options-reducer';

export interface AircraftMaintenanceCompanyOptionsState {
  aircraftMaintenanceCompanyOptions: fromAircraftMaintenanceCompanyOptions.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(
  state: AircraftMaintenanceCompanyOptionsState | undefined,
  action: Action
) {
  return combineReducers({
    aircraftMaintenanceCompanyOptions:
      fromAircraftMaintenanceCompanyOptions.aircraftMaintenanceCompanyOptionReducers,
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getAircraftMaintenanceCompaniesState =
  createFeatureSelector<AircraftMaintenanceCompanyOptionsState>(storeKey);

export const getAircraftMaintenanceCompanyOptionsEntitiesState = createSelector(
  getAircraftMaintenanceCompaniesState,
  state => state.aircraftMaintenanceCompanyOptions
);

export const { selectAll: getAllAircraftMaintenanceCompanyOptions } =
  fromAircraftMaintenanceCompanyOptions.aircraftMaintenanceCompanyOptionsAdapter.getSelectors(
    getAircraftMaintenanceCompanyOptionsEntitiesState
  );

export const getAircraftMaintenanceCompanyOptionById = (id: number) =>
  createSelector(
    getAircraftMaintenanceCompanyOptionsEntitiesState,
    fromAircraftMaintenanceCompanyOptions.getAircraftMaintenanceCompanyOptionById(
      id
    )
  );
