import * as fromUsers from './users-from-directory-reducer';
import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';

export interface UsersFromDirectoryState {
  usersFromDirectory: fromUsers.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(
  state: UsersFromDirectoryState | undefined,
  action: Action
) {
  return combineReducers({
    usersFromDirectory: fromUsers.userFromDirectoryReducers,
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getUsersState = createFeatureSelector<UsersFromDirectoryState>(
  'users-from-directory'
);

export const getUsersEntitiesState = createSelector(
  getUsersState,
  state => state.usersFromDirectory
);

export const { selectAll: getAllUsersFromDirectory } =
  fromUsers.usersFromDirectoryAdapter.getSelectors(getUsersEntitiesState);
