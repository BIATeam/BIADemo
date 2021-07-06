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

export const setDefaultSite = createAction('[Domain Sites] Set default site', props<{ id: number }>());

export const setDefaultSiteSuccess = createAction('[Domain Sites] Set default site success');

export const failure = createAction('[Domain Sites] Failure', props<{ error: any }>());
