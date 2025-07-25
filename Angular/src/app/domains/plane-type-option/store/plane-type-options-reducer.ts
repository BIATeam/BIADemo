import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { DomainPlaneTypeOptionsActions } from './plane-type-options-actions';

// This adapter will allow is to manipulate planeTypes (mostly CRUD operations)
export const planeTypeOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (planeType: OptionDto) => planeType.id,
  sortComparer: false,
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<PlaneType> {
//   ids: string[] | number[];
//   entities: { [id: string]: PlaneType };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export type State = EntityState<OptionDto>;

export const INIT_STATE: State = planeTypeOptionsAdapter.getInitialState({
  // additional props default values here
});

export const planeTypeOptionReducers = createReducer<State>(
  INIT_STATE,
  on(DomainPlaneTypeOptionsActions.loadAllSuccess, (state, { planeTypes }) =>
    planeTypeOptionsAdapter.setAll(planeTypes, state)
  )
);

export const getPlaneTypeOptionById = (id: number) => (state: State) =>
  state.entities[id];
