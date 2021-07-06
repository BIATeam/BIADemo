import { createAction, props } from '@ngrx/store';
import { View } from '../model/view';
import { DefaultView } from '../model/default-view';
import { SiteDefaultView } from '../model/site-default-view';
import { SiteView } from '../model/site-view';
import { AssignViewToSite } from '../model/assign-view-to-site';

export const loadAllView = createAction('[Views] Load all');

export const loadAllSuccess = createAction('[Views] Load all success', props<{ views: View[] }>());

export const assignViewToSite = createAction('[Views] Assign view to site', props<AssignViewToSite>());

export const removeUserView = createAction('[Views] Remove user view', props<{ id: number }>());

export const setDefaultUserView = createAction('[Views] Set default user view', props<DefaultView>());

export const addUserView = createAction('[Views] Add user view', props<View>());

export const updateUserView = createAction('[Views] Update user view', props<View>());

export const updateSiteView = createAction('[Views] Update site view', props<SiteView>());

export const addUserViewSuccess = createAction('[Views] Add user view success', props<View>());

export const removeSiteView = createAction('[Views] Remove site view', props<{ id: number }>());

export const setDefaultSiteView = createAction('[Views] Set default site view', props<SiteDefaultView>());

export const addSiteView = createAction('[Views] Add site view', props<SiteView>());

export const addSiteViewSuccess = createAction('[Views] Add site view success', props<SiteView>());

export const failure = createAction('[Views] Failure', props<{ error: any }>());

export const openViewDialog = createAction('[Views] Open view dialog', props<{ tableStateKey: string }>());

export const closeViewDialog = createAction('[Views] Close view dialog');

export const setViewSuccess = createAction('[Views] set view success', props<View>());

