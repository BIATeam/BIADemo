import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { DomainPartOptionsActions } from './part-options-actions';

// This adapter will allow is to manipulate parts (mostly CRUD operations)
export const partOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (part: OptionDto) => part.id,
  sortComparer: false,
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Part> {
//   ids: string[] | number[];
//   entities: { [id: string]: Part };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export type State = EntityState<OptionDto>;

export const INIT_STATE: State = partOptionsAdapter.getInitialState({
  // additional props default values here
});

export const partOptionReducers = createReducer<State>(
  INIT_STATE,
  on(DomainPartOptionsActions.loadAllSuccess, (state, { parts }) =>
    partOptionsAdapter.setAll(parts, state)
  )
  // on(loadSuccess, (state, { part }) => partOptionsAdapter.upsertOne(part, state))
);

export const getPartOptionById = (id: number) => (state: State) =>
  state.entities[id];
