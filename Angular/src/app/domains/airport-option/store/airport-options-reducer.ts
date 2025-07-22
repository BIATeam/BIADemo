import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'biang/models';
import { DomainAirportOptionsActions } from './airport-options-actions';

// This adapter will allow is to manipulate airports (mostly CRUD operations)
export const airportOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (airport: OptionDto) => airport.id,
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

export type State = EntityState<OptionDto>;

export const INIT_STATE: State = airportOptionsAdapter.getInitialState({
  // additional props default values here
});

export const airportOptionReducers = createReducer<State>(
  INIT_STATE,
  on(DomainAirportOptionsActions.loadAllSuccess, (state, { airports }) =>
    airportOptionsAdapter.setAll(airports, state)
  )
  // on(loadSuccess, (state, { airport }) => airportOptionsAdapter.upsertOne(airport, state))
);

export const getAirportOptionById = (id: number) => (state: State) =>
  state.entities[id];
