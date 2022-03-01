import { createAction, props } from '@ngrx/store';
import { Site } from '../model/site';

export const loadAllSites = createAction('[Domain Sites] Load all');


export const load = createAction('[Domain Sites] Load', props<{ id: number }>());

export const loadAllSuccess = createAction('[Domain Sites] Load all success', props<{ sites: Site[] }>());

export const loadAllSitesByUser = createAction('[Domain Sites] Load all by user', props<{ userId: number }>());

export const loadAllSitesByUserSuccess = createAction(
  '[Domain Sites] Load all sites by user success',
  props<{ sites: Site[] }>()
);

export const loadSuccess = createAction('[Domain Sites] Load success', props<{ site: Site }>());

export const setDefaultTeam = createAction('[Domain Sites] Set default site', props<{ teamTypeId: number, teamId: number }>());

export const setDefaultTeamSuccess = createAction('[Domain Sites] Set default site success');

export const failure = createAction('[Domain Sites] Failure', props<{ error: any }>());

export const setDefaultRoles = createAction('[Domain Role Options] Set default role', props<{ teamId: number, roleIds: number[]  }>());

export const setDefaultRolesSuccess = createAction('[Domain Role Options] Set default role success');
