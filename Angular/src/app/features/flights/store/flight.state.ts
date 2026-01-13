import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { flightCRUDConfiguration } from '../flight.constants';
import { Flight } from '../model/flight';
import * as fromFlights from './flights-reducer';

export namespace FeatureFlightsStore {
  export interface FlightsState {
    flights: fromFlights.State;
  }

  /** Provide reducers with AoT-compilation compliance */
  export function reducers(state: FlightsState | undefined, action: Action) {
    return combineReducers({
      flights: fromFlights.flightReducers,
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  export const getFlightsState = createFeatureSelector<FlightsState>(
    flightCRUDConfiguration.storeKey
  );

  export const getFlightsEntitiesState = createSelector(
    getFlightsState,
    state => state.flights
  );

  export const getFlightsTotalCount = createSelector(
    getFlightsEntitiesState,
    state => state.totalCount
  );

  export const getCurrentFlight = createSelector(
    getFlightsEntitiesState,
    state => state.currentItem ?? <Flight>{ id: '' }
  );

  export const getLastLazyLoadEvent = createSelector(
    getFlightsEntitiesState,
    state => state.lastLazyLoadEvent
  );

  export const getFlightLoadingGet = createSelector(
    getFlightsEntitiesState,
    state => state.loadingGet
  );

  export const getFlightLoadingGetAll = createSelector(
    getFlightsEntitiesState,
    state => state.loadingGetAll
  );

  export const { selectAll: getAllFlights } =
    fromFlights.flightsAdapter.getSelectors(getFlightsEntitiesState);

  export const getFlightById = (id: number) =>
    createSelector(getFlightsEntitiesState, fromFlights.getFlightById(id));
}
