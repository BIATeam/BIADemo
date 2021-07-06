import * as fromAirports from './airports-reducer';
import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';

export interface AirportsState {
  airports: fromAirports.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: AirportsState | undefined, action: Action) {
  return combineReducers({
    airports: fromAirports.airportReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getAirportsState = createFeatureSelector<AirportsState>('airports');

export const getAirportsEntitiesState = createSelector(
  getAirportsState,
  (state) => state.airports
);

export const getAirportsTotalCount = createSelector(
  getAirportsEntitiesState,
  (state) => state.totalCount
);

export const getCurrentAirport = createSelector(
  getAirportsEntitiesState,
  (state) => state.currentAirport
);

export const getLastLazyLoadEvent = createSelector(
  getAirportsEntitiesState,
  (state) => state.lastLazyLoadEvent
);

export const getAirportLoadingGet = createSelector(
  getAirportsEntitiesState,
  (state) => state.loadingGet
);

export const getAirportLoadingGetAll = createSelector(
  getAirportsEntitiesState,
  (state) => state.loadingGetAll
);

export const getDisplayEditDialog = createSelector(
  getAirportsEntitiesState,
  (state) => state.displayEditDialog
);

export const getDisplayNewDialog = createSelector(
  getAirportsEntitiesState,
  (state) => state.displayNewDialog
);

export const { selectAll: getAllAirports } = fromAirports.airportsAdapter.getSelectors(getAirportsEntitiesState);

export const getAirportById = (id: number) =>
  createSelector(
    getAirportsEntitiesState,
    fromAirports.getAirportById(id)
  );
