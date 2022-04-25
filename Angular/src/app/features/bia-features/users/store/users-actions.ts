import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { User } from '../model/user';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { storeKey } from '../user.constants';

export namespace FeatureUsersActions
{
  export const loadAllByPost = createAction('[' + storeKey +'] Load all by post', props<{ event: LazyLoadEvent }>());

  export const load = createAction('[' + storeKey +'] Load', props<{ id: number }>());
  
  export const create = createAction('[' + storeKey +'] Create', props<{ user: User }>());
  
  export const update = createAction('[' + storeKey +'] Update', props<{ user: User }>());
  
  export const remove = createAction('[' + storeKey +'] Remove', props<{ id: number }>());
  
  export const multiRemove = createAction('[' + storeKey +'] Multi Remove', props<{ ids: number[] }>());
  
  export const loadAllByPostSuccess = createAction(
    '[' + storeKey +'] Load all by post success',
    props<{ result: DataResult<User[]>; event: LazyLoadEvent }>()
  );
  
  export const loadSuccess = createAction('[' + storeKey +'] Load success', props<{ user: User }>());
  
  export const failure = createAction('[' + storeKey +'] Failure', props<{ error: any }>());

  export const synchronize = createAction('[' + storeKey +'] Synchronize');
}