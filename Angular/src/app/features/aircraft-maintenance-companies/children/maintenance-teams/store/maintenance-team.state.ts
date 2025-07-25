import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import { maintenanceTeamCRUDConfiguration } from '../maintenance-team.constants';
import { MaintenanceTeam } from '../model/maintenance-team';
import * as fromMaintenanceTeams from './maintenance-teams-reducer';

export namespace FeatureMaintenanceTeamsStore {
  export interface MaintenanceTeamsState {
    maintenanceTeams: fromMaintenanceTeams.State;
  }

  /** Provide reducers with AoT-compilation compliance */
  export function reducers(
    state: MaintenanceTeamsState | undefined,
    action: Action
  ) {
    return combineReducers({
      maintenanceTeams: fromMaintenanceTeams.maintenanceTeamReducers,
    })(state, action);
  }

  /**
   * The createFeatureSelector function selects a piece of state from the root of the state object.
   * This is used for selecting feature states that are loaded eagerly or lazily.
   */

  export const getMaintenanceTeamsState =
    createFeatureSelector<MaintenanceTeamsState>(
      maintenanceTeamCRUDConfiguration.storeKey
    );

  export const getMaintenanceTeamsEntitiesState = createSelector(
    getMaintenanceTeamsState,
    state => state.maintenanceTeams
  );

  export const getMaintenanceTeamsTotalCount = createSelector(
    getMaintenanceTeamsEntitiesState,
    state => state.totalCount
  );

  export const getCurrentMaintenanceTeam = createSelector(
    getMaintenanceTeamsEntitiesState,
    state => state.currentItem ?? <MaintenanceTeam>{}
  );

  export const getLastLazyLoadEvent = createSelector(
    getMaintenanceTeamsEntitiesState,
    state => state.lastLazyLoadEvent
  );

  export const getMaintenanceTeamLoadingGet = createSelector(
    getMaintenanceTeamsEntitiesState,
    state => state.loadingGet
  );

  export const getMaintenanceTeamLoadingGetAll = createSelector(
    getMaintenanceTeamsEntitiesState,
    state => state.loadingGetAll
  );

  export const { selectAll: getAllMaintenanceTeams } =
    fromMaintenanceTeams.maintenanceTeamsAdapter.getSelectors(
      getMaintenanceTeamsEntitiesState
    );

  export const getMaintenanceTeamById = (id: number) =>
    createSelector(
      getMaintenanceTeamsEntitiesState,
      fromMaintenanceTeams.getMaintenanceTeamById(id)
    );
}
