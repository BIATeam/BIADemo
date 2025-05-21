import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { engineCRUDConfiguration } from '../engine.constants';
import { Engine } from '../model/engine';
import * as fromEngines from './engines-reducer';

export namespace FeatureEnginesStore {
  export interface EnginesState {
    engines: fromEngines.State;
  }

  /** Provide reducers with AoT-compilation compliance */
  export function reducers(state: EnginesState | undefined, action: Action) {
    return combineReducers({
      engines: fromEngines.engineReducers,
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  export const getEnginesState = createFeatureSelector<EnginesState>(
    engineCRUDConfiguration.storeKey
  );

  export const getEnginesEntitiesState = createSelector(
    getEnginesState,
    state => state.engines
  );

  export const getEnginesTotalCount = createSelector(
    getEnginesEntitiesState,
    state => state.totalCount
  );

  export const getCurrentEngine = createSelector(
    getEnginesEntitiesState,
    state => state.currentItem ?? <Engine>{}
  );

  export const getLastLazyLoadEvent = createSelector(
    getEnginesEntitiesState,
    state => state.lastLazyLoadEvent
  );

  export const getEngineLoadingGet = createSelector(
    getEnginesEntitiesState,
    state => state.loadingGet
  );

  export const getEngineLoadingGetAll = createSelector(
    getEnginesEntitiesState,
    state => state.loadingGetAll
  );

  export const { selectAll: getAllEngines } =
    fromEngines.enginesAdapter.getSelectors(getEnginesEntitiesState);

  export const getEngineById = (id: number) =>
    createSelector(getEnginesEntitiesState, fromEngines.getEngineById(id));
}
