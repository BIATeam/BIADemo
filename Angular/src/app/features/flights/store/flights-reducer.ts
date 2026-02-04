import { CrudState, DEFAULT_CRUD_STATE } from '@bia-team/bia-ng/models';
import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { Flight } from '../model/flight';
import { FeatureFlightsActions } from './flights-actions';

// This adapter will allow is to manipulate flights (mostly CRUD operations)
export const flightsAdapter = createEntityAdapter<Flight>({
  selectId: (flight: Flight) => flight.id,
  sortComparer: false,
});

export interface State extends CrudState<Flight>, EntityState<Flight> {
  // additional props here
}

export const INIT_STATE: State = flightsAdapter.getInitialState({
  ...DEFAULT_CRUD_STATE(),
  // additional props default values here
});

export const flightReducers = createReducer<State>(
  INIT_STATE,
  on(FeatureFlightsActions.clearAll, state => {
    const stateUpdated = flightsAdapter.removeAll(state);
    stateUpdated.totalCount = 0;
    return stateUpdated;
  }),
  on(FeatureFlightsActions.clearCurrent, state => {
    return { ...state, currentItem: <Flight>{ id: '' } };
  }),
  on(FeatureFlightsActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureFlightsActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(FeatureFlightsActions.save, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureFlightsActions.loadAllByPostSuccess, (state, { result, event }) => {
    const stateUpdated = flightsAdapter.setAll(result.data, state);
    stateUpdated.totalCount = result.totalCount;
    stateUpdated.lastLazyLoadEvent = event;
    stateUpdated.loadingGetAll = false;
    return stateUpdated;
  }),
  on(FeatureFlightsActions.loadSuccess, (state, { flight }) => {
    return { ...state, currentItem: flight, loadingGet: false };
  }),
  on(FeatureFlightsActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getFlightById = (id: number) => (state: State) =>
  state.entities[id];
