import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { Engine } from '../model/engine';
import { FeatureEnginesActions } from './engines-actions';

// This adapter will allow is to manipulate engines (mostly CRUD operations)
export const enginesAdapter = createEntityAdapter<Engine>({
  selectId: (engine: Engine) => engine.id,
  sortComparer: false,
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Engine> {
//   ids: string[] | number[];
//   entities: { [id: string]: Engine };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<Engine> {
  // additional props here
  totalCount: number;
  currentEngine: Engine;
  lastLazyLoadEvent: TableLazyLoadEvent;
  loadingGet: boolean;
  loadingGetAll: boolean;
}

export const INIT_STATE: State = enginesAdapter.getInitialState({
  // additional props default values here
  totalCount: 0,
  currentEngine: <Engine>{},
  lastLazyLoadEvent: <TableLazyLoadEvent>{},
  loadingGet: false,
  loadingGetAll: false,
});

export const engineReducers = createReducer<State>(
  INIT_STATE,
  on(FeatureEnginesActions.clearAll, state => {
    const stateUpdated = enginesAdapter.removeAll(state);
    stateUpdated.totalCount = 0;
    return stateUpdated;
  }),
  on(FeatureEnginesActions.clearCurrent, state => {
    return { ...state, currentEngine: <Engine>{} };
  }),
  on(FeatureEnginesActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureEnginesActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(FeatureEnginesActions.loadAllByPostSuccess, (state, { result, event }) => {
    const stateUpdated = enginesAdapter.setAll(result.data, state);
    stateUpdated.totalCount = result.totalCount;
    stateUpdated.lastLazyLoadEvent = event;
    stateUpdated.loadingGetAll = false;
    return stateUpdated;
  }),
  on(FeatureEnginesActions.loadSuccess, (state, { engine }) => {
    return { ...state, currentEngine: engine, loadingGet: false };
  }),
  on(FeatureEnginesActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getEngineById = (id: number) => (state: State) =>
  state.entities[id];
