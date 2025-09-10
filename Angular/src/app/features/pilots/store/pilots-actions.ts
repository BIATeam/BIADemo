import { createAction, props } from '@ngrx/store';
import { DataResult } from 'packages/bia-ng/models/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { Pilot } from '../model/pilot';
import { pilotCRUDConfiguration } from '../pilot.constants';

export namespace FeaturePilotsActions {
  export const loadAllByPost = createAction(
    '[' + pilotCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + pilotCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + pilotCRUDConfiguration.storeKey + '] Create',
    props<{ pilot: Pilot }>()
  );

  export const update = createAction(
    '[' + pilotCRUDConfiguration.storeKey + '] Update',
    props<{ pilot: Pilot }>()
  );

  export const save = createAction(
    '[' + pilotCRUDConfiguration.storeKey + '] Save',
    props<{ pilots: Pilot[] }>()
  );

  export const remove = createAction(
    '[' + pilotCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + pilotCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' + pilotCRUDConfiguration.storeKey + '] Load all by post success',
    props<{ result: DataResult<Pilot[]>; event: TableLazyLoadEvent }>()
  );

  export const loadSuccess = createAction(
    '[' + pilotCRUDConfiguration.storeKey + '] Load success',
    props<{ pilot: Pilot }>()
  );

  export const failure = createAction(
    '[' + pilotCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + pilotCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + pilotCRUDConfiguration.storeKey + '] Clear current'
  );

  export const updateFixedStatus = createAction(
    '[' + pilotCRUDConfiguration.storeKey + '] Update fixed status',
    props<{ id: number; isFixed: boolean }>()
  );
}
