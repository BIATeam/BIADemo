import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { storeKey } from '../team-option.contants';
import * as fromTeamOptions from './team-options-reducer';

export interface TeamOptionsState {
  teamOptions: fromTeamOptions.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: TeamOptionsState | undefined, action: Action) {
  return combineReducers({
    teamOptions: fromTeamOptions.teamOptionReducers,
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getTeamsState = createFeatureSelector<TeamOptionsState>(storeKey);

export const getTeamOptionsEntitiesState = createSelector(
  getTeamsState,
  state => state.teamOptions
);

export const { selectAll: getAllTeamOptions } =
  fromTeamOptions.teamOptionsAdapter.getSelectors(getTeamOptionsEntitiesState);

export const getTeamOptionById = (id: number) =>
  createSelector(
    getTeamOptionsEntitiesState,
    fromTeamOptions.getTeamOptionById(id)
  );
