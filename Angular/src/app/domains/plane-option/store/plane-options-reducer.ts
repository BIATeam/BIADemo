import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'bia-ng/models';
import { DomainPlaneOptionsActions } from './plane-options-actions';

// This adapter will allow is to manipulate planes (mostly CRUD operations)
export const planeOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (plane: OptionDto) => plane.id,
  sortComparer: false,
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Plane> {
//   ids: string[] | number[];
//   entities: { [id: string]: Plane };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export type State = EntityState<OptionDto>;

export const INIT_STATE: State = planeOptionsAdapter.getInitialState({
  // additional props default values here
});

export const planeOptionReducers = createReducer<State>(
  INIT_STATE,
  on(DomainPlaneOptionsActions.loadAllSuccess, (state, { planes }) =>
    planeOptionsAdapter.setAll(planes, state)
  )
  // on(loadSuccess, (state, { plane }) => planeOptionsAdapter.upsertOne(plane, state))
);

export const getPlaneOptionById = (id: number) => (state: State) =>
  state.entities[id];
