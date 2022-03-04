import { createAction, props } from '@ngrx/store';
import { View } from '../model/view';
import { DefaultView } from '../model/default-view';
import { TeamDefaultView } from '../model/team-default-view';
import { TeamView } from '../model/team-view';
import { AssignViewToTeam } from '../model/assign-view-to-team';

export const loadAllView = createAction('[Views] Load all');

export const loadAllSuccess = createAction('[Views] Load all success', props<{ views: View[] }>());

export const assignViewToTeam = createAction('[Views] Assign view to team', props<AssignViewToTeam>());

export const removeUserView = createAction('[Views] Remove user view', props<{ id: number }>());

export const setDefaultUserView = createAction('[Views] Set default user view', props<DefaultView>());

export const addUserView = createAction('[Views] Add user view', props<View>());

export const updateUserView = createAction('[Views] Update user view', props<View>());

export const updateTeamView = createAction('[Views] Update team view', props<TeamView>());

export const addUserViewSuccess = createAction('[Views] Add user view success', props<View>());

export const removeTeamView = createAction('[Views] Remove team view', props<{ id: number }>());

export const setDefaultTeamView = createAction('[Views] Set default team view', props<TeamDefaultView>());

export const addTeamView = createAction('[Views] Add team view', props<TeamView>());

export const addTeamViewSuccess = createAction('[Views] Add team view success', props<TeamView>());

export const failure = createAction('[Views] Failure', props<{ error: any }>());

export const openViewDialog = createAction('[Views] Open view dialog', props<{ tableStateKey: string }>());

export const closeViewDialog = createAction('[Views] Close view dialog');

export const setViewSuccess = createAction('[Views] set view success', props<View>());

