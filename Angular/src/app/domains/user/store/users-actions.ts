import { createAction, props } from '@ngrx/store';
import { User } from '../model/user';

export const loadAllByFilter = createAction('[Domain Users] Load all by filter', props<{ filter: string }>());

export const loadAllSuccess = createAction('[Domain Users] Load all success', props<{ users: User[] }>());

export const failure = createAction('[Domain Users] Failure', props<{ error: any }>());
