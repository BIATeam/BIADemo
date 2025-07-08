import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { storeKey } from '../plane-option.contants';
import * as fromPlaneOptions from './plane-options-reducer';

export interface PlaneOptionsState {
  planeOptions: fromPlaneOptions.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: PlaneOptionsState | undefined, action: Action) {
  return combineReducers({
    planeOptions: fromPlaneOptions.planeOptionReducers,
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getPlanesState =
  createFeatureSelector<PlaneOptionsState>(storeKey);

export const getPlaneOptionsEntitiesState = createSelector(
  getPlanesState,
  state => state.planeOptions
);

export const { selectAll: getAllPlaneOptions } =
  fromPlaneOptions.planeOptionsAdapter.getSelectors(
    getPlaneOptionsEntitiesState
  );

export const getPlaneOptionById = (id: number) =>
  createSelector(
    getPlaneOptionsEntitiesState,
    fromPlaneOptions.getPlaneOptionById(id)
  );
