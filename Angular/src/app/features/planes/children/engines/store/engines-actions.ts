import { DataResult } from '@bia-team/bia-ng/models';
import { createAction, props } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { engineCRUDConfiguration } from '../engine.constants';
import { Engine } from '../model/engine';

export namespace FeatureEnginesActions {
  export const loadAllByPost = createAction(
    '[' + engineCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + engineCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + engineCRUDConfiguration.storeKey + '] Create',
    props<{ engine: Engine }>()
  );

  export const update = createAction(
    '[' + engineCRUDConfiguration.storeKey + '] Update',
    props<{ engine: Engine }>()
  );

  export const save = createAction(
    '[' + engineCRUDConfiguration.storeKey + '] Save',
    props<{ engines: Engine[] }>()
  );

  export const remove = createAction(
    '[' + engineCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + engineCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' + engineCRUDConfiguration.storeKey + '] Load all by post success',
    props<{ result: DataResult<Engine[]>; event: TableLazyLoadEvent }>()
  );

  export const loadSuccess = createAction(
    '[' + engineCRUDConfiguration.storeKey + '] Load success',
    props<{ engine: Engine }>()
  );

  export const failure = createAction(
    '[' + engineCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + engineCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + engineCRUDConfiguration.storeKey + '] Clear current'
  );

  export const updateFixedStatus = createAction(
    '[' + engineCRUDConfiguration.storeKey + '] Update fixed status',
    props<{ id: number; isFixed: boolean }>()
  );
}
