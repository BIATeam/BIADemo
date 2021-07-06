import * as fromAirportOptions from './airport-options-reducer';
import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';

export interface AirportOptionsState {
  airportOptions: fromAirportOptions.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: AirportOptionsState | undefined, action: Action) {
  return combineReducers({
    airportOptions: fromAirportOptions.airportOptionReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getAirportsState = createFeatureSelector<AirportOptionsState>('domain-airports');

export const getAirportOptionsEntitiesState = createSelector(
  getAirportsState,
  (state) => state.airportOptions
);

export const { selectAll: getAllAirportOptions } = fromAirportOptions.airportOptionsAdapter.getSelectors(
  getAirportOptionsEntitiesState
);

export const getAirportOptionById = (id: number) =>
  createSelector(
    getAirportOptionsEntitiesState,
    fromAirportOptions.getAirportOptionById(id)
  );


















