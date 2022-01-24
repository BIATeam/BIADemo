import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import {
  loadSuccess,
  loadAllByPostSuccess,
  loadAllByPost,
  load,
  failure
} from './planes-actions';
import { LazyLoadEvent } from 'primeng/api';
import { Plane } from '../model/plane';

// This adapter will allow is to manipulate planes (mostly CRUD operations)
export const planesAdapter = createEntityAdapter<Plane>({
  selectId: (plane: Plane) => plane.id,
  sortComparer: false
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

export interface State extends EntityState<Plane> {
  // additional props here
  totalCount: number;
  currentPlane: Plane;
  lastLazyLoadEvent: LazyLoadEvent;
  loadingGet: boolean;
  loadingGetAll: boolean;
}

export const INIT_STATE: State = planesAdapter.getInitialState({
  // additional props default values here
  totalCount: 0,
  currentPlane: <Plane>{},
  lastLazyLoadEvent: <LazyLoadEvent>{},
  loadingGet: false,
  loadingGetAll: false,
});

export const planeReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllByPost, (state, { event }) => {
    return { ...state, loadingGetAll: true };
  }),
  on(load, (state) => {
    return { ...state, loadingGet: true };
  }),
  on(loadAllByPostSuccess, (state, { result, event }) => {
    const stateUpdated = planesAdapter.setAll(result.data, state);
    stateUpdated.totalCount = result.totalCount;
    stateUpdated.lastLazyLoadEvent = event;
    stateUpdated.loadingGetAll = false;
    return stateUpdated;
  }),
  on(loadSuccess, (state, { plane }) => {
    return { ...state, currentPlane: plane, loadingGet: false };
  }),
  on(failure, (state, { error }) => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  }),
);

export const getPlaneById = (id: number) => (state: State) => state.entities[id];
