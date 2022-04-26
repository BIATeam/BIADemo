import * as fromUserOptions from './user-options-reducer';
import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';

export interface UserOptionsState {
  userOptions: fromUserOptions.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: UserOptionsState | undefined, action: Action) {
  return combineReducers({
    userOptions: fromUserOptions.userOptionReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getUsersState = createFeatureSelector<UserOptionsState>('domain-user-options');

export const getUserOptionsEntitiesState = createSelector(
  getUsersState,
  (state) => state.userOptions
);

export const { selectAll: getAllUserOptions } = fromUserOptions.userOptionsAdapter.getSelectors(
  getUserOptionsEntitiesState
);

export const getUserOptionById = (id: number) =>
  createSelector(
    getUserOptionsEntitiesState,
    fromUserOptions.getUserOptionById(id)
  );

export const getLastUsersAdded = createSelector(
  getUserOptionsEntitiesState,
  (state) => state.lastUsersAdded
);















