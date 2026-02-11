import { CrudState, DEFAULT_CRUD_STATE } from '@bia-team/bia-ng/models';
import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { MaintenanceTeam } from '../model/maintenance-team';
import { FeatureMaintenanceTeamsActions } from './maintenance-teams-actions';

// This adapter will allow is to manipulate maintenanceTeams (mostly CRUD operations)
export const maintenanceTeamsAdapter = createEntityAdapter<MaintenanceTeam>({
  selectId: (maintenanceTeam: MaintenanceTeam) => maintenanceTeam.id,
  sortComparer: false,
});

export interface State
  extends CrudState<MaintenanceTeam>, EntityState<MaintenanceTeam> {
  // additional props here
}

export const INIT_STATE: State = maintenanceTeamsAdapter.getInitialState({
  ...DEFAULT_CRUD_STATE(),
  // additional props default values here
});

export const maintenanceTeamReducers = createReducer<State>(
  INIT_STATE,
  on(FeatureMaintenanceTeamsActions.clearAll, state => {
    const stateUpdated = maintenanceTeamsAdapter.removeAll(state);
    stateUpdated.totalCount = 0;
    return stateUpdated;
  }),
  on(FeatureMaintenanceTeamsActions.clearCurrent, state => {
    return { ...state, currentItem: <MaintenanceTeam>{} };
  }),
  on(FeatureMaintenanceTeamsActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureMaintenanceTeamsActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(
    FeatureMaintenanceTeamsActions.loadAllByPostSuccess,
    (state, { result, event }) => {
      const stateUpdated = maintenanceTeamsAdapter.setAll(result.data, state);
      stateUpdated.totalCount = result.totalCount;
      stateUpdated.lastLazyLoadEvent = event;
      stateUpdated.loadingGetAll = false;
      return stateUpdated;
    }
  ),
  on(
    FeatureMaintenanceTeamsActions.loadSuccess,
    (state, { maintenanceTeam }) => {
      return { ...state, currentItem: maintenanceTeam, loadingGet: false };
    }
  ),
  on(FeatureMaintenanceTeamsActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getMaintenanceTeamById = (id: number) => (state: State) =>
  state.entities[id];
