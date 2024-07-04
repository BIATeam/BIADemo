import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { User } from '../model/user';
import { userCRUDConfiguration } from '../user.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export namespace FeatureUsersActions {
  export const loadAllByPost = createAction(
    '[' + userCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: LazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + userCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + userCRUDConfiguration.storeKey + '] Create',
    props<{ user: User }>()
  );

  export const update = createAction(
    '[' + userCRUDConfiguration.storeKey + '] Update',
    props<{ user: User }>()
  );

  export const save = createAction(
    '[' + userCRUDConfiguration.storeKey + '] Save',
    props<{ users: User[] }>()
  );

  export const remove = createAction(
    '[' + userCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + userCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' + userCRUDConfiguration.storeKey + '] Load all by post success',
    props<{ result: DataResult<User[]>; event: LazyLoadEvent }>()
  );

  export const loadSuccess = createAction(
    '[' + userCRUDConfiguration.storeKey + '] Load success',
    props<{ user: User }>()
  );

  export const failure = createAction(
    '[' + userCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const synchronize = createAction(
    '[' + userCRUDConfiguration.storeKey + '] Synchronize'
  );

  export const clearAll = createAction(
    '[' + userCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + userCRUDConfiguration.storeKey + '] Clear current'
  );
}
