import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { loadAllSuccess } from './airport-options-actions';

// This adapter will allow is to manipulate airports (mostly CRUD operations)
export const airportOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (airport: OptionDto) => airport.id,
  sortComparer: false
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

export interface State extends EntityState<OptionDto> {
  // additional props here
}

export const INIT_STATE: State = airportOptionsAdapter.getInitialState({
  // additional props default values here
});

export const airportOptionReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllSuccess, (state, { airports }) => airportOptionsAdapter.setAll(airports, state)),
  // on(loadSuccess, (state, { airport }) => airportOptionsAdapter.upsertOne(airport, state))
);

export const getAirportOptionById = (id: number) => (state: State) => state.entities[id];


















