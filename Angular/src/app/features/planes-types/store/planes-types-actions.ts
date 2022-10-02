import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { PlaneType } from '../model/plane-type';
import { PlaneTypeCRUDConfiguration } from '../plane-type.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export namespace FeaturePlanesTypesActions
{
  export const loadAllByPost = createAction('[' + PlaneTypeCRUDConfiguration.storeKey +'] Load all by post', props<{ event: LazyLoadEvent }>());

  export const load = createAction('[' + PlaneTypeCRUDConfiguration.storeKey +'] Load', props<{ id: number }>());
  
  export const create = createAction('[' + PlaneTypeCRUDConfiguration.storeKey +'] Create', props<{ planeType: PlaneType }>());
  
  export const update = createAction('[' + PlaneTypeCRUDConfiguration.storeKey +'] Update', props<{ planeType: PlaneType }>());
  
  export const remove = createAction('[' + PlaneTypeCRUDConfiguration.storeKey +'] Remove', props<{ id: number }>());
  
  export const multiRemove = createAction('[' + PlaneTypeCRUDConfiguration.storeKey +'] Multi Remove', props<{ ids: number[] }>());
  
  export const loadAllByPostSuccess = createAction(
    '[' + PlaneTypeCRUDConfiguration.storeKey +'] Load all by post success',
    props<{ result: DataResult<PlaneType[]>; event: LazyLoadEvent }>()
  );
  
  export const loadSuccess = createAction('[' + PlaneTypeCRUDConfiguration.storeKey +'] Load success', props<{ planeType: PlaneType }>());
  
  export const failure = createAction('[' + PlaneTypeCRUDConfiguration.storeKey +'] Failure', props<{ error: any }>());
}