import { createAction, props } from '@ngrx/store';
import { UserFilter } from 'src/app/features/users/model/UserFilter';
import { UserFromAD } from '../model/user-from-AD';

export const loadAllByFilter = createAction(
  '[Domain Users From AD] Load all in AD by filter',
  props<{ userFilter: UserFilter }>()
);

export const loadAllSuccess = createAction('[Domain Users From AD] Load all success', props<{ users: UserFromAD[] }>());

export const failure = createAction('[Domain Users From AD] Failure', props<{ error: any }>());
