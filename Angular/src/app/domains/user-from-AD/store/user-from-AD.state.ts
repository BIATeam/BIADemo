import * as fromUsers from './users-from-AD-reducer';
import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';

export interface UsersFromADState {
  usersFromAD: fromUsers.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: UsersFromADState | undefined, action: Action) {
  return combineReducers({
    usersFromAD: fromUsers.userFromADReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getUsersState = createFeatureSelector<UsersFromADState>('domain-users-from-AD');

export const getUsersEntitiesState = createSelector(
  getUsersState,
  (state) => state.usersFromAD
);

export const { selectAll: getAllUsersFromAD } = fromUsers.usersFromADAdapter.getSelectors(
  getUsersEntitiesState
);
