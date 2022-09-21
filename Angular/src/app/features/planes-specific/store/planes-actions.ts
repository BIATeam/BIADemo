import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Plane } from '../model/plane';
import { PlaneCRUDConfiguration } from '../plane.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export namespace FeaturePlanesActions
{
  export const loadAllByPost = createAction('[' + PlaneCRUDConfiguration.storeKey +'] Load all by post', props<{ event: LazyLoadEvent }>());

  export const load = createAction('[' + PlaneCRUDConfiguration.storeKey +'] Load', props<{ id: number }>());
  
  export const create = createAction('[' + PlaneCRUDConfiguration.storeKey +'] Create', props<{ plane: Plane }>());
  
  export const update = createAction('[' + PlaneCRUDConfiguration.storeKey +'] Update', props<{ plane: Plane }>());
  
  export const remove = createAction('[' + PlaneCRUDConfiguration.storeKey +'] Remove', props<{ id: number }>());
  
  export const multiRemove = createAction('[' + PlaneCRUDConfiguration.storeKey +'] Multi Remove', props<{ ids: number[] }>());
  
  export const loadAllByPostSuccess = createAction(
    '[' + PlaneCRUDConfiguration.storeKey +'] Load all by post success',
    props<{ result: DataResult<Plane[]>; event: LazyLoadEvent }>()
  );
  
  export const loadSuccess = createAction('[' + PlaneCRUDConfiguration.storeKey +'] Load success', props<{ plane: Plane }>());
  
  export const failure = createAction('[' + PlaneCRUDConfiguration.storeKey +'] Failure', props<{ error: any }>());
}