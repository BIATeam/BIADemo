import { createAction, props } from '@ngrx/store';
import { HistoricalEntryDto } from 'packages/bia-ng/models/dto/historical-entry-dto';
import { DataResult } from 'packages/bia-ng/models/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { Plane } from '../model/plane';
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
    props<{ plane: Plane }>()
  );

  export const update = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Update',
    props<{ plane: Plane }>()
  );

  export const save = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Save',
    props<{ planes: Plane[] }>()
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
    props<{ plane: Plane }>()
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

  export const updateFixedStatus = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Update fixed status',
    props<{ id: number; isFixed: boolean }>()
  );

  export const loadHistorical = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Load historical',
    props<{ id: number }>()
  );

  export const loadHistoricalSuccess = createAction(
    '[' + planeCRUDConfiguration.storeKey + '] Load historical success',
    props<{ historical: HistoricalEntryDto[] }>()
  );
}
