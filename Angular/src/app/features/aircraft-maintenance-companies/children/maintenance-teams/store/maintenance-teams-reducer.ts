import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { FeatureMaintenanceTeamsActions } from './maintenance-teams-actions';
import { LazyLoadEvent } from 'primeng/api';
import { MaintenanceTeam } from '../model/maintenance-team';

// This adapter will allow is to manipulate maintenanceTeams (mostly CRUD operations)
export const maintenanceTeamsAdapter = createEntityAdapter<MaintenanceTeam>({
  selectId: (maintenanceTeam: MaintenanceTeam) => maintenanceTeam.id,
  sortComparer: false
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<MaintenanceTeam> {
//   ids: string[] | number[];
//   entities: { [id: string]: MaintenanceTeam };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<MaintenanceTeam> {
  // additional props here
  totalCount: number;
  currentMaintenanceTeam: MaintenanceTeam;
  lastLazyLoadEvent: LazyLoadEvent;
  loadingGet: boolean;
  loadingGetAll: boolean;
}

export const INIT_STATE: State = maintenanceTeamsAdapter.getInitialState({
  // additional props default values here
  totalCount: 0,
  currentMaintenanceTeam: <MaintenanceTeam>{},
  lastLazyLoadEvent: <LazyLoadEvent>{},
  loadingGet: false,
  loadingGetAll: false,
});

export const maintenanceTeamReducers = createReducer<State>(
  INIT_STATE,
  on(FeatureMaintenanceTeamsActions.loadAllByPost, (state, { event }) => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureMaintenanceTeamsActions.load, (state) => {
    return { ...state, loadingGet: true };
  }),
  on(FeatureMaintenanceTeamsActions.loadAllByPostSuccess, (state, { result, event }) => {
    const stateUpdated = maintenanceTeamsAdapter.setAll(result.data, state);
    stateUpdated.totalCount = result.totalCount;
    stateUpdated.lastLazyLoadEvent = event;
    stateUpdated.loadingGetAll = false;
    return stateUpdated;
  }),
  on(FeatureMaintenanceTeamsActions.loadSuccess, (state, { maintenanceTeam }) => {
    return { ...state, currentMaintenanceTeam: maintenanceTeam, loadingGet: false };
  }),
  on(FeatureMaintenanceTeamsActions.failure, (state, { error }) => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  }),
);

export const getMaintenanceTeamById = (id: number) => (state: State) => state.entities[id];
