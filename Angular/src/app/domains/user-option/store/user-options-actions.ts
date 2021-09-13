import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export const loadAllUserOptions = createAction('[Domain Users] Load all');

export const loadAllSuccess = createAction('[Domain Users] Load all success', props<{ users: OptionDto[] }>());
/*
export const load = createAction('[Domain Users] Load', props<{ id: number }>());

export const loadSuccess = createAction('[Domain Users] Load success', props<{ user: UserOption }>());
*/
export const failure = createAction('[Domain Users] Failure', props<{ error: any }>());


















