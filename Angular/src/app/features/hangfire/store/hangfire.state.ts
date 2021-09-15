import * as fromHangfire from './hangfire-reducer';
import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';

export interface HangfireState {
  hangfire: fromHangfire.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: HangfireState | undefined, action: Action) {
  return combineReducers({
    hangfire: fromHangfire.hangfireReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getHangfireState = createFeatureSelector<HangfireState>('hangfire');

export const getHangfireEntitiesState = createSelector(
  getHangfireState,
  (state) => state.hangfire
);

export const getLastLazyLoadEvent = createSelector(
  getHangfireEntitiesState,
  (state) => state.lastLazyLoadEvent
);

export const getHangfireLoadingGet = createSelector(
  getHangfireEntitiesState,
  (state) => state.loadingGet
);
