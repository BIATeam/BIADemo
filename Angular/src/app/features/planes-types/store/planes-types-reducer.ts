import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import {
  loadSuccess,
  loadAllByPostSuccess,
  loadAllByPost,
  load,
  openDialogEdit,
  closeDialogEdit,
  openDialogNew,
  closeDialogNew,
  failure
} from './planes-types-actions';
import { LazyLoadEvent } from 'primeng/api';
import { PlaneType } from '../model/plane-type';

// This adapter will allow is to manipulate planesTypes (mostly CRUD operations)
export const planesTypesAdapter = createEntityAdapter<PlaneType>({
  selectId: (planeType: PlaneType) => planeType.id,
  sortComparer: false
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
  displayEditDialog: boolean;
  displayNewDialog: boolean;
}

export const INIT_STATE: State = planesTypesAdapter.getInitialState({
  // additional props default values here
  totalCount: 0,
  currentPlaneType: <PlaneType>{},
  lastLazyLoadEvent: <LazyLoadEvent>{},
  loadingGet: false,
  loadingGetAll: false,
  displayEditDialog: false,
  displayNewDialog: false
});

export const planeTypeReducers = createReducer<State>(
  INIT_STATE,
  on(openDialogNew, (state) => {
    return { ...state, displayNewDialog: true };
  }),
  on(closeDialogNew, (state) => {
    return { ...state, displayNewDialog: false };
  }),
  on(openDialogEdit, (state) => {
    return { ...state, displayEditDialog: true };
  }),
  on(closeDialogEdit, (state) => {
    return { ...state, displayEditDialog: false };
  }),
  on(loadAllByPost, (state, { event }) => {
    return { ...state, loadingGetAll: true };
  }),
  on(load, (state) => {
    return { ...state, loadingGet: true };
  }),
  on(loadAllByPostSuccess, (state, { result, event }) => {
    const stateUpdated = planesTypesAdapter.setAll(result.data, state);
    stateUpdated.totalCount = result.totalCount;
    stateUpdated.lastLazyLoadEvent = event;
    stateUpdated.loadingGetAll = false;
    return stateUpdated;
  }),
  on(loadSuccess, (state, { planeType }) => {
    return { ...state, currentPlaneType: planeType, loadingGet: false };
  }),
  on(failure, (state, { error }) => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  }),
);

export const getPlaneTypeById = (id: number) => (state: State) => state.entities[id];
