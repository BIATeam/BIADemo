import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { Team } from '../model/team';
import { loadAllTeamsSuccess } from './teams-actions';

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
}

export const INIT_STATE: State = teamsAdapter.getInitialState({
  // additional props default values here
});

export const teamReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllTeamsSuccess, (state, { teams }) => teamsAdapter.setAll(teams, state)),
  // on(loadSuccess, (state, { team }) => teamsAdapter.upsertOne(team, state))
);

export const getTeamById = (id: number) => (state: State) => state.entities[id];