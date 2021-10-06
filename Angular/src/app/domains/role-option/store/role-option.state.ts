import * as fromRoleOptions from './role-options-reducer';
import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';

export interface RoleOptionsState {
  roleOptions: fromRoleOptions.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: RoleOptionsState | undefined, action: Action) {
  return combineReducers({
    roleOptions: fromRoleOptions.roleOptionReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getRolesState = createFeatureSelector<RoleOptionsState>('domain-role-options');

export const getRoleOptionsEntitiesState = createSelector(
  getRolesState,
  (state) => state.roleOptions
);

export const { selectAll: getAllRoleOptions } = fromRoleOptions.roleOptionsAdapter.getSelectors(
  getRoleOptionsEntitiesState
);

export const getRoleOptionById = (id: number) =>
  createSelector(
    getRoleOptionsEntitiesState,
    fromRoleOptions.getRoleOptionById(id)
  );


















