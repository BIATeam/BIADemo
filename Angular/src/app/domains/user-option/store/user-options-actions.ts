import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export const loadAllUserOptions = createAction('[Domain User Options] Load all');

export const loadAllSuccess = createAction('[Domain User Options] Load all success', props<{ users: OptionDto[] }>());

export const loadAllByFilter = createAction('[Domain User Options] Load all by filter', props<{ filter: string }>());

/*
export const load = createAction('[Domain User Options] Load', props<{ id: number }>());

export const loadSuccess = createAction('[Domain User Options] Load success', props<{ user: UserOption }>());
*/
export const failure = createAction('[Domain User Options] Failure', props<{ error: any }>());


















