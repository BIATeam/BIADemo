import * as fromUsers from './users-reducer';
import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';

export interface UsersState {
  users: fromUsers.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: UsersState | undefined, action: Action) {
  return combineReducers({
    users: fromUsers.userReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getUsersState = createFeatureSelector<UsersState>('users');

export const getUsersEntitiesState = createSelector(
  getUsersState,
  (state) => state.users
);

export const getUsersTotalCount = createSelector(
  getUsersEntitiesState,
  (state) => state.totalCount
);

export const getCurrentUser = createSelector(
  getUsersEntitiesState,
  (state) => state.currentUser
);

export const getLastLazyLoadEvent = createSelector(
  getUsersEntitiesState,
  (state) => state.lastLazyLoadEvent
);

export const { selectAll: getAllUsers } = fromUsers.usersAdapter.getSelectors(
  getUsersEntitiesState
);

export const getUserById = (id: number) =>
  createSelector(
    getUsersEntitiesState,
    fromUsers.getUserById(id)
  );
