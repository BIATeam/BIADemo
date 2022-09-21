import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Site } from '../model/site';
import { SiteCRUDConfiguration } from '../site.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export namespace FeatureSitesActions
{
  export const loadAllByPost = createAction('[' + SiteCRUDConfiguration.storeKey +'] Load all by post', props<{ event: LazyLoadEvent }>());

  export const load = createAction('[' + SiteCRUDConfiguration.storeKey +'] Load', props<{ id: number }>());
  
  export const create = createAction('[' + SiteCRUDConfiguration.storeKey +'] Create', props<{ site: Site }>());
  
  export const update = createAction('[' + SiteCRUDConfiguration.storeKey +'] Update', props<{ site: Site }>());
  
  export const remove = createAction('[' + SiteCRUDConfiguration.storeKey +'] Remove', props<{ id: number }>());
  
  export const multiRemove = createAction('[' + SiteCRUDConfiguration.storeKey +'] Multi Remove', props<{ ids: number[] }>());
  
  export const loadAllByPostSuccess = createAction(
    '[' + SiteCRUDConfiguration.storeKey +'] Load all by post success',
    props<{ result: DataResult<Site[]>; event: LazyLoadEvent }>()
  );
  
  export const loadSuccess = createAction('[' + SiteCRUDConfiguration.storeKey +'] Load success', props<{ site: Site }>());
  
  export const failure = createAction('[' + SiteCRUDConfiguration.storeKey +'] Failure', props<{ error: any }>());
}