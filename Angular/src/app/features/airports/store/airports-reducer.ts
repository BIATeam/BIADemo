import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { Airport } from '../model/airport';
import { FeatureAirportsActions } from './airports-actions';

// This adapter will allow is to manipulate airports (mostly CRUD operations)
export const airportsAdapter = createEntityAdapter<Airport>({
  selectId: (airport: Airport) => airport.id,
  sortComparer: false,
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Airport> {
//   ids: string[] | number[];
//   entities: { [id: string]: Airport };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<Airport> {
  // additional props here
  totalCount: number;
  currentAirport: Airport;
  lastLazyLoadEvent: TableLazyLoadEvent;
  loadingGet: boolean;
  loadingGetAll: boolean;
}

export const INIT_STATE: State = airportsAdapter.getInitialState({
  // additional props default values here
  totalCount: 0,
  currentAirport: <Airport>{},
  lastLazyLoadEvent: <TableLazyLoadEvent>{},
  loadingGet: false,
  loadingGetAll: false,
});

export const airportReducers = createReducer<State>(
  INIT_STATE,
  on(FeatureAirportsActions.clearAll, state => {
    const stateUpdated = airportsAdapter.removeAll(state);
    stateUpdated.totalCount = 0;
    return stateUpdated;
  }),
  on(FeatureAirportsActions.clearCurrent, state => {
    return { ...state, currentAirport: <Airport>{} };
  }),
  on(FeatureAirportsActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureAirportsActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(
    FeatureAirportsActions.loadAllByPostSuccess,
    (state, { result, event }) => {
      const stateUpdated = airportsAdapter.setAll(result.data, state);
      stateUpdated.totalCount = result.totalCount;
      stateUpdated.lastLazyLoadEvent = event;
      stateUpdated.loadingGetAll = false;
      return stateUpdated;
    }
  ),
  on(FeatureAirportsActions.loadSuccess, (state, { airport }) => {
    return { ...state, currentAirport: airport, loadingGet: false };
  }),
  on(FeatureAirportsActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getAirportById = (id: number) => (state: State) =>
  state.entities[id];
