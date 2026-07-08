import { createAction, props } from '@ngrx/store';
import { DataResult } from 'packages/bia-ng/models/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { PlaneType } from '../model/plane-type';
import { planeTypeCRUDConfiguration } from '../plane-type.constants';

export namespace FeaturePlanesTypesActions {
  export const loadAllByPost = createAction(
    '[' + planeTypeCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + planeTypeCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + planeTypeCRUDConfiguration.storeKey + '] Create',
    props<{ planeType: PlaneType }>()
  );

  export const update = createAction(
    '[' + planeTypeCRUDConfiguration.storeKey + '] Update',
    props<{ planeType: PlaneType }>()
  );

  export const remove = createAction(
    '[' + planeTypeCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + planeTypeCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' + planeTypeCRUDConfiguration.storeKey + '] Load all by post success',
    props<{ result: DataResult<PlaneType[]>; event: TableLazyLoadEvent }>()
  );

  export const loadSuccess = createAction(
    '[' + planeTypeCRUDConfiguration.storeKey + '] Load success',
    props<{ planeType: PlaneType }>()
  );

  export const failure = createAction(
    '[' + planeTypeCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + planeTypeCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + planeTypeCRUDConfiguration.storeKey + '] Clear current'
  );
}
