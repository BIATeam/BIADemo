import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export const loadAllRoleOptions = createAction('[Domain Roles] Load all');

export const loadAllSuccess = createAction('[Domain Roles] Load all success', props<{ roles: OptionDto[] }>());
/*
export const load = createAction('[Domain Roles] Load', props<{ id: number }>());

export const loadSuccess = createAction('[Domain Roles] Load success', props<{ role: RoleOption }>());
*/
export const failure = createAction('[Domain Roles] Failure', props<{ error: any }>());


















