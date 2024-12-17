import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { storeKey } from '../plane-type-option.contants';
import * as fromPlaneTypeOptions from './plane-type-options-reducer';

export interface PlaneTypeOptionsState {
  planeTypeOptions: fromPlaneTypeOptions.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(
  state: PlaneTypeOptionsState | undefined,
  action: Action
) {
  return combineReducers({
    planeTypeOptions: fromPlaneTypeOptions.planeTypeOptionReducers,
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getPlanesTypesState =
  createFeatureSelector<PlaneTypeOptionsState>(storeKey);

export const getPlaneTypeOptionsEntitiesState = createSelector(
  getPlanesTypesState,
  state => state.planeTypeOptions
);

export const { selectAll: getAllPlaneTypeOptions } =
  fromPlaneTypeOptions.planeTypeOptionsAdapter.getSelectors(
    getPlaneTypeOptionsEntitiesState
  );

export const getPlaneTypeOptionById = (id: number) =>
  createSelector(
    getPlaneTypeOptionsEntitiesState,
    fromPlaneTypeOptions.getPlaneTypeOptionById(id)
  );
