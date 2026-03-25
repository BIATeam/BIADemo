import {
  CrudState,
  DEFAULT_CRUD_STATE,
  HistoricalEntryDto,
} from '@bia-team/bia-ng/models';
import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { Plane } from '../model/plane';
import { FeaturePlanesActions } from './planes-actions';

// This adapter will allow is to manipulate planes (mostly CRUD operations)
export const planesAdapter = createEntityAdapter<Plane>({
  selectId: (plane: Plane) => plane.id,
  sortComparer: false,
});

export interface State extends CrudState<Plane>, EntityState<Plane> {
  // additional props here
  currentItemHistorical: HistoricalEntryDto[];
}

export const INIT_STATE: State = planesAdapter.getInitialState({
  ...DEFAULT_CRUD_STATE(),
  currentItemHistorical: [],
  // additional props default values here
});

export const planeReducers = createReducer<State>(
  INIT_STATE,
  on(FeaturePlanesActions.clearAll, state => {
    const stateUpdated = planesAdapter.removeAll(state);
    stateUpdated.totalCount = 0;
    return stateUpdated;
  }),
  on(FeaturePlanesActions.clearCurrent, state => {
    return { ...state, currentItem: <Plane>{}, currentItemHistorical: [] };
  }),
  on(FeaturePlanesActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeaturePlanesActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(FeaturePlanesActions.save, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeaturePlanesActions.loadAllByPostSuccess, (state, { result, event }) => {
    const stateUpdated = planesAdapter.setAll(result.data, state);
    stateUpdated.totalCount = result.totalCount;
    stateUpdated.lastLazyLoadEvent = event;
    stateUpdated.loadingGetAll = false;
    return stateUpdated;
  }),
  on(FeaturePlanesActions.loadSuccess, (state, { plane }) => {
    return { ...state, currentItem: plane, loadingGet: false };
  }),
  on(FeaturePlanesActions.loadHistoricalSuccess, (state, { historical }) => {
    return { ...state, currentItemHistorical: historical };
  }),
  on(FeaturePlanesActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getPlaneById = (id: number) => (state: State) =>
  state.entities[id];
