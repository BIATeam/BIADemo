import { createAction, props } from '@ngrx/store';
import { User } from 'src/app/domains/user/model/user';
import { LazyLoadEvent } from 'primeng/api';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { UserFromAD } from 'src/app/domains/user-from-AD/model/user-from-AD';

export const loadAllByPost = createAction('[Users] Load all by post', props<{ event: LazyLoadEvent }>());

export const load = createAction('[Users] Load', props<{ id: number }>());

export const create = createAction('[Users] Create', props<{ user: User }>());

export const update = createAction('[Users] Update', props<{ user: User }>());

export const remove = createAction('[Users] Remove', props<{ id: number }>());

export const multiRemove = createAction('[Users] Multi Remove', props<{ ids: number[] }>());

export const save = createAction('[Users] Save', props<{ usersFromAD: UserFromAD[] }>());

export const synchronize = createAction('[Users] Synchronize');

export const loadAllSuccess = createAction('[Users] Load all success', props<{ users: User[] }>());

export const loadAllByPostSuccess = createAction(
  '[Users] Load all by post success',
  props<{ result: DataResult<User[]>; event: LazyLoadEvent }>()
);

export const loadSuccess = createAction('[Users] Load success', props<{ user: User }>());

export const createSuccess = createAction('[Users] Create success', props<{ user: User }>());

export const updateSuccess = createAction('[Users] Update success', props<{ user: User }>());

export const removeSuccess = createAction('[Users] Remove success', props<{ id: number }>());

export const failure = createAction('[Users] Failure', props<{ err: { concern: 'CREATE' | 'PATCH'; error: any } }>());
