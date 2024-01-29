import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Engine } from '../model/engine';
import { EngineCRUDConfiguration } from '../engine.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export namespace FeatureEnginesActions
{
  export const clearAll = createAction('[' + EngineCRUDConfiguration.storeKey +'] Clear all in state');
  
  export const clearCurrent = createAction('[' + EngineCRUDConfiguration.storeKey +'] Clear current');
  
  export const loadAllByPost = createAction('[' + EngineCRUDConfiguration.storeKey +'] Load all by post', props<{ event: LazyLoadEvent }>());

  export const load = createAction('[' + EngineCRUDConfiguration.storeKey +'] Load', props<{ id: number }>());
  
  export const create = createAction('[' + EngineCRUDConfiguration.storeKey +'] Create', props<{ engine: Engine }>());
  
  export const update = createAction('[' + EngineCRUDConfiguration.storeKey +'] Update', props<{ engine: Engine }>());
  
  export const remove = createAction('[' + EngineCRUDConfiguration.storeKey +'] Remove', props<{ id: number }>());
  
  export const multiRemove = createAction('[' + EngineCRUDConfiguration.storeKey +'] Multi Remove', props<{ ids: number[] }>());
  
  export const loadAllByPostSuccess = createAction(
    '[' + EngineCRUDConfiguration.storeKey +'] Load all by post success',
    props<{ result: DataResult<Engine[]>; event: LazyLoadEvent }>()
  );
  
  export const loadSuccess = createAction('[' + EngineCRUDConfiguration.storeKey +'] Load success', props<{ engine: Engine }>());
  
  export const failure = createAction('[' + EngineCRUDConfiguration.storeKey +'] Failure', props<{ error: any }>());
}