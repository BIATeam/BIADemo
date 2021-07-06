import * as fromMembers from './members-reducer';
import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';

export interface MembersState {
  members: fromMembers.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: MembersState | undefined, action: Action) {
  return combineReducers({
    members: fromMembers.memberReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getMembersState = createFeatureSelector<MembersState>('members');

export const getMembersEntitiesState = createSelector(
  getMembersState,
  (state) => state.members
);

export const getMembersTotalCount = createSelector(
  getMembersEntitiesState,
  (state) => state.totalCount
);

export const getCurrentMember = createSelector(
  getMembersEntitiesState,
  (state) => state.currentMember
);

export const getLastLazyLoadEvent = createSelector(
  getMembersEntitiesState,
  (state) => state.lastLazyLoadEvent
);

export const getMemberLoadingGet = createSelector(
  getMembersEntitiesState,
  (state) => state.loadingGet
);

export const getMemberLoadingGetAll = createSelector(
  getMembersEntitiesState,
  (state) => state.loadingGetAll
);

export const getDisplayEditDialog = createSelector(
  getMembersEntitiesState,
  (state) => state.displayEditDialog
);

export const getDisplayNewDialog = createSelector(
  getMembersEntitiesState,
  (state) => state.displayNewDialog
);

export const { selectAll: getAllMembers } = fromMembers.membersAdapter.getSelectors(getMembersEntitiesState);

export const getMemberById = (id: number) =>
  createSelector(
    getMembersEntitiesState,
    fromMembers.getMemberById(id)
  );
