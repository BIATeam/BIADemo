import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Member, Members } from '../model/member';
import { MemberCRUDConfiguration } from '../member.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export namespace FeatureMembersActions
{
  export const loadAllByPost = createAction('[' + MemberCRUDConfiguration.storeKey +'] Load all by post', props<{ event: LazyLoadEvent }>());

  export const load = createAction('[' + MemberCRUDConfiguration.storeKey +'] Load', props<{ id: number }>());
  
  export const create = createAction('[' + MemberCRUDConfiguration.storeKey +'] Create', props<{ member: Member }>());

  export const createMulti = createAction('[Members] Create multi', props<{ members: Members }>());
  
  export const update = createAction('[' + MemberCRUDConfiguration.storeKey +'] Update', props<{ member: Member }>());
  
  export const remove = createAction('[' + MemberCRUDConfiguration.storeKey +'] Remove', props<{ id: number }>());
  
  export const multiRemove = createAction('[' + MemberCRUDConfiguration.storeKey +'] Multi Remove', props<{ ids: number[] }>());
  
  export const loadAllByPostSuccess = createAction(
    '[' + MemberCRUDConfiguration.storeKey +'] Load all by post success',
    props<{ result: DataResult<Member[]>; event: LazyLoadEvent }>()
  );
  
  export const loadSuccess = createAction('[' + MemberCRUDConfiguration.storeKey +'] Load success', props<{ member: Member }>());
  
  export const failure = createAction('[' + MemberCRUDConfiguration.storeKey +'] Failure', props<{ error: any }>());
}