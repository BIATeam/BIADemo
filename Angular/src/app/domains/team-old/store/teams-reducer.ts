import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { loadAllTeamsByUser, loadAllTeamsByUserSuccess, loadAllSuccess, loadSuccess } from './teams-actions';
import { Team } from '../model/team';

// This adapter will allow is to manipulate teams (mostly CRUD operations)
export const teamsAdapter = createEntityAdapter<Team>({
  selectId: (team: Team) => team.id,
  sortComparer: false
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Team> {
//   ids: string[] | number[];
//   entities: { [id: string]: Team };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<Team> {
  // additional props here
  userTeams: Team[] | null;
}

export const INIT_STATE: State = teamsAdapter.getInitialState({
  // additional props default values here
  userTeams: null
});

export const teamReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllSuccess, (state, { teams }) => teamsAdapter.setAll(teams, state)),
  on(loadSuccess, (state, { team }) => teamsAdapter.upsertOne(team, state)),
  on(loadAllTeamsByUser, (state, {}) => {
    return { ...state, userTeams: null };
  }),
  on(loadAllTeamsByUserSuccess, (state, { teams }) => {
    return { ...state, userTeams: teams };
  })
);

export const getTeamById = (id: number) => (state: State) => state.entities[id];
