import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Member, Members } from '../model/member';
import { memberCRUDConfiguration } from '../member.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export namespace FeatureMembersActions {
  export const loadAllByPost = createAction(
    '[' + memberCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: LazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + memberCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + memberCRUDConfiguration.storeKey + '] Create',
    props<{ member: Member }>()
  );

  export const createMulti = createAction(
    '[Members] Create multi',
    props<{ members: Members }>()
  );

  export const update = createAction(
    '[' + memberCRUDConfiguration.storeKey + '] Update',
    props<{ member: Member }>()
  );

  export const remove = createAction(
    '[' + memberCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + memberCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' + memberCRUDConfiguration.storeKey + '] Load all by post success',
    props<{ result: DataResult<Member[]>; event: LazyLoadEvent }>()
  );

  export const loadSuccess = createAction(
    '[' + memberCRUDConfiguration.storeKey + '] Load success',
    props<{ member: Member }>()
  );

  export const failure = createAction(
    '[' + memberCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + memberCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + memberCRUDConfiguration.storeKey + '] Clear current'
  );
}
