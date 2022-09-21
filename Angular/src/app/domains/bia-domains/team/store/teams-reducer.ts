import { EntityState, createEntityAdapter, Update } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { Team } from '../model/team';
import { DomainTeamsActions } from './teams-actions';

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
  on(DomainTeamsActions.loadAllSuccess, (state, { teams }) => teamsAdapter.setAll(teams, state)),
  on(DomainTeamsActions.setDefaultTeamSuccess, (state, { teamTypeId,teamId }) => 
    {
      let updates: Update<Team>[] = [];
      for (let key in state.entities) {
        let value = state.entities[key];
        // Use `key` and `value`
        if (value?.teamTypeId == teamTypeId)
        {
          if (value.id == teamId)
          {
            updates.push({id: key, changes: {isDefault: true}})
          }
          else if (value.isDefault)
          {
            updates.push({id: key, changes: {isDefault: false}})
          }
        }
      }
      return teamsAdapter.updateMany(updates, state);
    }
  ),
  on(DomainTeamsActions.setDefaultRolesSuccess, (state, { teamId,roleIds }) => 
    {
      for (let key in state.entities) {
        let value = state.entities[key];
        // Use `key` and `value`
        if (value?.id == teamId)
        {
          //let roles = value.roles.map(role => ({role, isDefault: roleIds.indexOf(role.id) > -1}));
          let roles = [...value.roles];
          roles.forEach(role => role.isDefault = (roleIds.indexOf(role.id) > -1))

          return teamsAdapter.updateOne({id:key, changes:{ roles : roles} }, state);
        }
      }
      return state;
    }
  )
  // on(loadSuccess, (state, { team }) => teamsAdapter.upsertOne(team, state))
);


export const getTeamById = (id: number) => (state: State) => state.entities[id];
