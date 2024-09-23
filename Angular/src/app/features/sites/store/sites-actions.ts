import { createAction, props } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { Site } from '../model/site';
import { siteCRUDConfiguration } from '../site.constants';

export namespace FeatureSitesActions {
  export const loadAllByPost = createAction(
    '[' + siteCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + siteCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + siteCRUDConfiguration.storeKey + '] Create',
    props<{ site: Site }>()
  );

  export const update = createAction(
    '[' + siteCRUDConfiguration.storeKey + '] Update',
    props<{ site: Site }>()
  );

  export const remove = createAction(
    '[' + siteCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + siteCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' + siteCRUDConfiguration.storeKey + '] Load all by post success',
    props<{ result: DataResult<Site[]>; event: TableLazyLoadEvent }>()
  );

  export const loadSuccess = createAction(
    '[' + siteCRUDConfiguration.storeKey + '] Load success',
    props<{ site: Site }>()
  );

  export const failure = createAction(
    '[' + siteCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + siteCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + siteCRUDConfiguration.storeKey + '] Clear current'
  );
}
