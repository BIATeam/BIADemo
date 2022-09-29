import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { User } from '../model/user';
import { UserCRUDConfiguration } from '../user.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export namespace FeatureUsersActions
{
  export const loadAllByPost = createAction('[' + UserCRUDConfiguration.storeKey +'] Load all by post', props<{ event: LazyLoadEvent }>());

  export const load = createAction('[' + UserCRUDConfiguration.storeKey +'] Load', props<{ id: number }>());
  
  export const create = createAction('[' + UserCRUDConfiguration.storeKey +'] Create', props<{ user: User }>());
  
  export const update = createAction('[' + UserCRUDConfiguration.storeKey +'] Update', props<{ user: User }>());
  
  export const remove = createAction('[' + UserCRUDConfiguration.storeKey +'] Remove', props<{ id: number }>());
  
  export const multiRemove = createAction('[' + UserCRUDConfiguration.storeKey +'] Multi Remove', props<{ ids: number[] }>());
  
  export const loadAllByPostSuccess = createAction(
    '[' + UserCRUDConfiguration.storeKey +'] Load all by post success',
    props<{ result: DataResult<User[]>; event: LazyLoadEvent }>()
  );
  
  export const loadSuccess = createAction('[' + UserCRUDConfiguration.storeKey +'] Load success', props<{ user: User }>());
  
  export const failure = createAction('[' + UserCRUDConfiguration.storeKey +'] Failure', props<{ error: any }>());

  export const synchronize = createAction('[' + UserCRUDConfiguration.storeKey +'] Synchronize');
}