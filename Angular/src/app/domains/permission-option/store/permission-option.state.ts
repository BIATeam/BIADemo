import * as fromPermissionOptions from './permission-options-reducer';
import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';

export interface PermissionOptionsState {
  permissionOptions: fromPermissionOptions.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: PermissionOptionsState | undefined, action: Action) {
  return combineReducers({
    permissionOptions: fromPermissionOptions.permissionOptionReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getPermissionsState = createFeatureSelector<PermissionOptionsState>('domain-permission-options');

export const getPermissionOptionsEntitiesState = createSelector(
  getPermissionsState,
  (state) => state.permissionOptions
);

export const { selectAll: getAllPermissionOptions } = fromPermissionOptions.permissionOptionsAdapter.getSelectors(
  getPermissionOptionsEntitiesState
);

export const getPermissionOptionById = (id: number) =>
  createSelector(
    getPermissionOptionsEntitiesState,
    fromPermissionOptions.getPermissionOptionById(id)
  );


















