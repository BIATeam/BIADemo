import { DataResult } from '@bia-team/bia-ng/models';
import { createAction, props } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { flightCRUDConfiguration } from '../flight.constants';
import { Flight } from '../model/flight';

export namespace FeatureFlightsActions {
  export const loadAllByPost = createAction(
    '[' + flightCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + flightCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + flightCRUDConfiguration.storeKey + '] Create',
    props<{ flight: Flight }>()
  );

  export const update = createAction(
    '[' + flightCRUDConfiguration.storeKey + '] Update',
    props<{ flight: Flight }>()
  );

  export const save = createAction(
    '[' + flightCRUDConfiguration.storeKey + '] Save',
    props<{ flights: Flight[] }>()
  );

  export const remove = createAction(
    '[' + flightCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + flightCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' + flightCRUDConfiguration.storeKey + '] Load all by post success',
    props<{ result: DataResult<Flight[]>; event: TableLazyLoadEvent }>()
  );

  export const loadSuccess = createAction(
    '[' + flightCRUDConfiguration.storeKey + '] Load success',
    props<{ flight: Flight }>()
  );

  export const failure = createAction(
    '[' + flightCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + flightCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + flightCRUDConfiguration.storeKey + '] Clear current'
  );

  export const updateFixedStatus = createAction(
    '[' + flightCRUDConfiguration.storeKey + '] Update fixed status',
    props<{ id: number; isFixed: boolean }>()
  );
}
