import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import { DomainAnnoucementTypeOptionsActions } from './annoucement-type-options-actions';

// This adapter will allow is to manipulate annoucementTypes (mostly CRUD operations)
export const annoucementTypeOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (annoucementType: OptionDto) => annoucementType.id,
  sortComparer: false,
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<AnnoucementType> {
//   ids: string[] | number[];
//   entities: { [id: string]: AnnoucementType };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export type State = EntityState<OptionDto>;

export const INIT_STATE: State = annoucementTypeOptionsAdapter.getInitialState({
  // additional props default values here
});

export const annoucementTypeOptionReducers = createReducer<State>(
  INIT_STATE,
  on(
    DomainAnnoucementTypeOptionsActions.loadAllSuccess,
    (state, { annoucementTypes }) =>
      annoucementTypeOptionsAdapter.setAll(annoucementTypes, state)
  )
);

export const getAnnoucementTypeOptionById = (id: number) => (state: State) =>
  state.entities[id];
