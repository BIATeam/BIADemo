import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Engine } from '../model/engine';
import { engineCRUDConfiguration } from '../engine.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export namespace FeatureEnginesActions {
  export const loadAllByPost = createAction(
    '[' + engineCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: LazyLoadEvent }>()
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
    props<{ result: DataResult<Engine[]>; event: LazyLoadEvent }>()
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
}
