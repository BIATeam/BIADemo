import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { FeaturePlanesTypesActions } from './planes-types-actions';
import { LazyLoadEvent } from 'primeng/api';
import { PlaneType } from '../model/plane-type';

// This adapter will allow is to manipulate planesTypes (mostly CRUD operations)
export const planesTypesAdapter = createEntityAdapter<PlaneType>({
  selectId: (planeType: PlaneType) => planeType.id,
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

export interface State extends EntityState<PlaneType> {
  // additional props here
  totalCount: number;
  currentPlaneType: PlaneType;
  lastLazyLoadEvent: LazyLoadEvent;
  loadingGet: boolean;
  loadingGetAll: boolean;
}

export const INIT_STATE: State = planesTypesAdapter.getInitialState({
  // additional props default values here
  totalCount: 0,
  currentPlaneType: <PlaneType>{},
  lastLazyLoadEvent: <LazyLoadEvent>{},
  loadingGet: false,
  loadingGetAll: false,
});

export const planeTypeReducers = createReducer<State>(
  INIT_STATE,
  on(FeaturePlanesTypesActions.loadAllByPost, (state, { event }) => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeaturePlanesTypesActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(
    FeaturePlanesTypesActions.loadAllByPostSuccess,
    (state, { result, event }) => {
      const stateUpdated = planesTypesAdapter.setAll(result.data, state);
      stateUpdated.totalCount = result.totalCount;
      stateUpdated.lastLazyLoadEvent = event;
      stateUpdated.loadingGetAll = false;
      return stateUpdated;
    }
  ),
  on(FeaturePlanesTypesActions.loadSuccess, (state, { planeType }) => {
    return { ...state, currentPlaneType: planeType, loadingGet: false };
  }),
  on(FeaturePlanesTypesActions.failure, (state, { error }) => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getPlaneTypeById = (id: number) => (state: State) =>
  state.entities[id];
