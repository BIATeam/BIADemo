import * as fromRoles from './roles-reducer';
import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';

export interface RolesState {
  roles: fromRoles.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: RolesState | undefined, action: Action) {
  return combineReducers({
    roles: fromRoles.roleReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getRolesState = createFeatureSelector<RolesState>('domain-roles');

export const getRolesEntitiesState = createSelector(
  getRolesState,
  (state) => state.roles
);

export const getMemberRoles = createSelector(
  getRolesState,
  (state) => state.roles?.memberRoles ?? []
);

export const { selectAll: getAllRoles } = fromRoles.rolesAdapter.getSelectors(
  getRolesEntitiesState
);

export const getRoleById = (id: number) =>
  createSelector(
    getRolesEntitiesState,
    fromRoles.getRoleById(id)
  );
