import { createAction, props } from '@ngrx/store';
import { DataResult } from 'packages/bia-ng/models/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { airportCRUDConfiguration } from '../airport.constants';
import { Airport } from '../model/airport';

export namespace FeatureAirportsActions {
  export const loadAllByPost = createAction(
    '[' + airportCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + airportCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + airportCRUDConfiguration.storeKey + '] Create',
    props<{ airport: Airport }>()
  );

  export const update = createAction(
    '[' + airportCRUDConfiguration.storeKey + '] Update',
    props<{ airport: Airport }>()
  );

  export const remove = createAction(
    '[' + airportCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + airportCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' + airportCRUDConfiguration.storeKey + '] Load all by post success',
    props<{ result: DataResult<Airport[]>; event: TableLazyLoadEvent }>()
  );

  export const loadSuccess = createAction(
    '[' + airportCRUDConfiguration.storeKey + '] Load success',
    props<{ airport: Airport }>()
  );

  export const failure = createAction(
    '[' + airportCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + airportCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + airportCRUDConfiguration.storeKey + '] Clear current'
  );
}
