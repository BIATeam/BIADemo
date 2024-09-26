import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { storeKey } from '../part-option.contants';
import * as fromPartOptions from './part-options-reducer';

export interface PartOptionsState {
  partOptions: fromPartOptions.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: PartOptionsState | undefined, action: Action) {
  return combineReducers({
    partOptions: fromPartOptions.partOptionReducers,
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getPartsState = createFeatureSelector<PartOptionsState>(storeKey);

export const getPartOptionsEntitiesState = createSelector(
  getPartsState,
  state => state.partOptions
);

export const { selectAll: getAllPartOptions } =
  fromPartOptions.partOptionsAdapter.getSelectors(getPartOptionsEntitiesState);

export const getPartOptionById = (id: number) =>
  createSelector(
    getPartOptionsEntitiesState,
    fromPartOptions.getPartOptionById(id)
  );
