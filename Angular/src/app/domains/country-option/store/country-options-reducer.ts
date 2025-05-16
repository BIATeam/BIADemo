import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { DomainCountryOptionsActions } from './country-options-actions';

// This adapter will allow is to manipulate countries (mostly CRUD operations)
export const countryOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (country: OptionDto) => country.id,
  sortComparer: false,
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Country> {
//   ids: string[] | number[];
//   entities: { [id: string]: Country };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export type State = EntityState<OptionDto>;

export const INIT_STATE: State = countryOptionsAdapter.getInitialState({
  // additional props default values here
});

export const countryOptionReducers = createReducer<State>(
  INIT_STATE,
  on(DomainCountryOptionsActions.loadAllSuccess, (state, { countries }) =>
    countryOptionsAdapter.setAll(countries, state)
  )
);

export const getCountryOptionById = (id: number) => (state: State) =>
  state.entities[id];
