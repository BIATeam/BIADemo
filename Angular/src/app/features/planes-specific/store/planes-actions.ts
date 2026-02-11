import { DataResult } from '@bia-team/bia-ng/models';
import { createAction, props } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { Plane } from '../model/plane';
import { PlaneSpecific } from '../model/plane-specific';
import { planeCRUDConfiguration } from '../plane.constants';

export namespace FeaturePlanesActions {
  export const loadAllByPost = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Create',
    props<{ plane: PlaneSpecific }>()
  );

  export const update = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Update',
    props<{ plane: PlaneSpecific }>()
  );

  export const remove = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Load all by post success',
    props<{ result: DataResult<Plane[]>; event: TableLazyLoadEvent }>()
  );

  export const loadSuccess = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Load success',
    props<{ plane: PlaneSpecific }>()
  );

  export const failure = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Clear current'
  );
}
