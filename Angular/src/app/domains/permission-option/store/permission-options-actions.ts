import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export const loadAllPermissionOptions = createAction('[Domain Permission Options] Load all');

export const loadAllPermissionOptionsSuccess = createAction(
    '[Domain Permission Options] Load all success', props<{ permissions: OptionDto[] }>());
/*
export const load = createAction('[Domain Permission Options] Load', props<{ id: number }>());

export const loadSuccess = createAction('[Domain Permission Options] Load success', props<{ permission: PermissionOption }>());
*/
export const failure = createAction('[Domain Permission Options] Failure', props<{ error: any }>());
















