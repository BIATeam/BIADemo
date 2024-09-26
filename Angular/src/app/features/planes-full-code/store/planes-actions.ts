import { createAction, props } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { Plane } from '../model/plane';
import { storeKey } from '../plane.constants';

export namespace FeaturePlanesActions {
  export const loadAllByPost = createAction(
    '[' + storeKey + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + storeKey + '] Create',
    props<{ plane: Plane }>()
  );

  export const update = createAction(
    '[' + storeKey + '] Update',
    props<{ plane: Plane }>()
  );

  export const remove = createAction(
    '[' + storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' + storeKey + '] Load all by post success',
    props<{ result: DataResult<Plane[]>; event: TableLazyLoadEvent }>()
  );

  export const loadSuccess = createAction(
    '[' + storeKey + '] Load success',
    props<{ plane: Plane }>()
  );

  export const failure = createAction(
    '[' + storeKey + '] Failure',
    props<{ error: any }>()
  );
}
