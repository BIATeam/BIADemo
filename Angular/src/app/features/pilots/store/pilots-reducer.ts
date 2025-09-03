import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import {
  CrudState,
  DEFAULT_CRUD_STATE,
} from 'packages/bia-ng/models/public-api';
import { Pilot } from '../model/pilot';
import { FeaturePilotsActions } from './pilots-actions';

// This adapter will allow is to manipulate pilots (mostly CRUD operations)
export const pilotsAdapter = createEntityAdapter<Pilot>({
  selectId: (pilot: Pilot) => pilot.id,
  sortComparer: false,
});

export interface State extends CrudState<Pilot>, EntityState<Pilot> {
  // additional props here
}

export const INIT_STATE: State = pilotsAdapter.getInitialState({
  ...DEFAULT_CRUD_STATE(),
  // additional props default values here
});

export const pilotReducers = createReducer<State>(
  INIT_STATE,
  on(FeaturePilotsActions.clearAll, state => {
    const stateUpdated = pilotsAdapter.removeAll(state);
    stateUpdated.totalCount = 0;
    return stateUpdated;
  }),
  on(FeaturePilotsActions.clearCurrent, state => {
    return { ...state, currentItem: <Pilot>{} };
  }),
  on(FeaturePilotsActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeaturePilotsActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(FeaturePilotsActions.save, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeaturePilotsActions.loadAllByPostSuccess, (state, { result, event }) => {
    const stateUpdated = pilotsAdapter.setAll(result.data, state);
    stateUpdated.totalCount = result.totalCount;
    stateUpdated.lastLazyLoadEvent = event;
    stateUpdated.loadingGetAll = false;
    return stateUpdated;
  }),
  on(FeaturePilotsActions.loadSuccess, (state, { pilot }) => {
    return { ...state, currentItem: pilot, loadingGet: false };
  }),
  on(FeaturePilotsActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getPilotById = (id: number) => (state: State) =>
  state.entities[id];
