import * as fromTeams from './teams-reducer';
import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';
import { storeKey } from '../team.contants';

export interface TeamsState {
  teams: fromTeams.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: TeamsState | undefined, action: Action) {
  return combineReducers({
    teams: fromTeams.teamReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getTeamsState = createFeatureSelector<TeamsState>(storeKey);

export const getTeamsEntitiesState = createSelector(
  getTeamsState,
  (state) => state.teams
);

export const { selectAll: getAllTeams } = fromTeams.teamsAdapter.getSelectors(
  getTeamsEntitiesState
);

export const getTeamById = (id: number) =>
  createSelector(
    getTeamsEntitiesState,
    fromTeams.getTeamById(id)
  );

export const getAllTeamsOfType = (teamTypeId: number) =>
  createSelector(
    getAllTeams,
    (teams) => teams.filter(team => team.teamTypeId === teamTypeId)
  );
















