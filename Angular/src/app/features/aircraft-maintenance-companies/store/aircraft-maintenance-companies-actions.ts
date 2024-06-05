import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { AircraftMaintenanceCompany } from '../model/aircraft-maintenance-company';
import { AircraftMaintenanceCompanyCRUDConfiguration } from '../aircraft-maintenance-company.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export namespace FeatureAircraftMaintenanceCompaniesActions {
  export const loadAllByPost = createAction(
    '[' +
      AircraftMaintenanceCompanyCRUDConfiguration.storeKey +
      '] Load all by post',
    props<{ event: LazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + AircraftMaintenanceCompanyCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + AircraftMaintenanceCompanyCRUDConfiguration.storeKey + '] Create',
    props<{ aircraftMaintenanceCompany: AircraftMaintenanceCompany }>()
  );

  export const update = createAction(
    '[' + AircraftMaintenanceCompanyCRUDConfiguration.storeKey + '] Update',
    props<{ aircraftMaintenanceCompany: AircraftMaintenanceCompany }>()
  );

  export const remove = createAction(
    '[' + AircraftMaintenanceCompanyCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' +
      AircraftMaintenanceCompanyCRUDConfiguration.storeKey +
      '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' +
      AircraftMaintenanceCompanyCRUDConfiguration.storeKey +
      '] Load all by post success',
    props<{
      result: DataResult<AircraftMaintenanceCompany[]>;
      event: LazyLoadEvent;
    }>()
  );

  export const loadSuccess = createAction(
    '[' +
      AircraftMaintenanceCompanyCRUDConfiguration.storeKey +
      '] Load success',
    props<{ aircraftMaintenanceCompany: AircraftMaintenanceCompany }>()
  );

  export const failure = createAction(
    '[' + AircraftMaintenanceCompanyCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' +
      AircraftMaintenanceCompanyCRUDConfiguration.storeKey +
      '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' +
      AircraftMaintenanceCompanyCRUDConfiguration.storeKey +
      '] Clear current'
  );
}
