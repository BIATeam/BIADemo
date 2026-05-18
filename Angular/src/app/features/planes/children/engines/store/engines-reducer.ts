import { CrudState, DEFAULT_CRUD_STATE } from '@bia-team/bia-ng/models';
import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { Engine } from '../model/engine';
import { FeatureEnginesActions } from './engines-actions';

// This adapter will allow is to manipulate engines (mostly CRUD operations)
export const enginesAdapter = createEntityAdapter<Engine>({
  selectId: (engine: Engine) => engine.id,
  sortComparer: false,
});

export interface State extends CrudState<Engine>, EntityState<Engine> {
  // additional props here
}

export const INIT_STATE: State = enginesAdapter.getInitialState({
  ...DEFAULT_CRUD_STATE(),
  // additional props default values here
});

export const engineReducers = createReducer<State>(
  INIT_STATE,
  on(FeatureEnginesActions.clearAll, state => {
    const stateUpdated = enginesAdapter.removeAll(state);
    stateUpdated.totalCount = 0;
    return stateUpdated;
  }),
  on(FeatureEnginesActions.clearCurrent, state => {
    return { ...state, currentItem: <Engine>{} };
  }),
  on(FeatureEnginesActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureEnginesActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(FeatureEnginesActions.save, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureEnginesActions.loadAllByPostSuccess, (state, { result, event }) => {
    const stateUpdated = enginesAdapter.setAll(result.data, state);
    stateUpdated.totalCount = result.totalCount;
    stateUpdated.lastLazyLoadEvent = event;
    stateUpdated.loadingGetAll = false;
    return stateUpdated;
  }),
  on(FeatureEnginesActions.loadSuccess, (state, { engine }) => {
    return { ...state, currentItem: engine, loadingGet: false };
  }),
  on(FeatureEnginesActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getEngineById = (id: number) => (state: State) =>
  state.entities[id];
