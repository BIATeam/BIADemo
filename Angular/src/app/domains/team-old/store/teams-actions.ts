import { createAction, props } from '@ngrx/store';
import { Team } from '../model/team';

export const loadAllTeams = createAction('[Domain Teams] Load all');


export const load = createAction('[Domain Teams] Load', props<{ id: number }>());

export const loadAllSuccess = createAction('[Domain Teams] Load all success', props<{ teams: Team[] }>());

export const loadAllTeamsByUser = createAction('[Domain Teams] Load all by user', props<{ userId: number }>());

export const loadAllTeamsByUserSuccess = createAction(
  '[Domain Teams] Load all teams by user success',
  props<{ teams: Team[] }>()
);

export const loadSuccess = createAction('[Domain Teams] Load success', props<{ team: Team }>());

export const setDefaultTeam = createAction('[Domain Teams] Set default team', props<{ teamTypeId: number, teamId: number }>());

export const setDefaultTeamSuccess = createAction('[Domain Teams] Set default team success');

export const failure = createAction('[Domain Teams] Failure', props<{ error: any }>());

export const setDefaultRoles = createAction('[Domain Role Options] Set default role', props<{ teamId: number, roleIds: number[]  }>());

export const setDefaultRolesSuccess = createAction('[Domain Role Options] Set default role success');
