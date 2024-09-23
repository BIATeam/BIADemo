import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { Plane } from '../model/plane';
import { PlaneSpecific } from '../model/plane-specific';
import { FeaturePlanesActions } from './planes-actions';

// This adapter will allow is to manipulate planes (mostly CRUD operations)
export const planesAdapter = createEntityAdapter<Plane>({
  selectId: (plane: Plane) => plane.id,
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

export interface State extends EntityState<Plane> {
  // additional props here
  totalCount: number;
  currentPlane: PlaneSpecific;
  lastLazyLoadEvent: TableLazyLoadEvent;
  loadingGet: boolean;
  loadingGetAll: boolean;
}

export const INIT_STATE: State = planesAdapter.getInitialState({
  // additional props default values here
  totalCount: 0,
  currentPlane: { engines: [] } as unknown as PlaneSpecific,
  lastLazyLoadEvent: <TableLazyLoadEvent>{},
  loadingGet: false,
  loadingGetAll: false,
});

export const planeReducers = createReducer<State>(
  INIT_STATE,
  on(FeaturePlanesActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeaturePlanesActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(FeaturePlanesActions.loadAllByPostSuccess, (state, { result, event }) => {
    const stateUpdated = planesAdapter.setAll(result.data, state);
    stateUpdated.totalCount = result.totalCount;
    stateUpdated.lastLazyLoadEvent = event;
    stateUpdated.loadingGetAll = false;
    return stateUpdated;
  }),
  on(FeaturePlanesActions.loadSuccess, (state, { plane }) => {
    return { ...state, currentPlane: plane, loadingGet: false };
  }),
  on(FeaturePlanesActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getPlaneById = (id: number) => (state: State) =>
  state.entities[id];
