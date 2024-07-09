import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { AircraftMaintenanceCompany } from '../model/aircraft-maintenance-company';
import { aircraftMaintenanceCompanyCRUDConfiguration } from '../aircraft-maintenance-company.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export namespace FeatureAircraftMaintenanceCompaniesActions {
  export const loadAllByPost = createAction(
    '[' +
      aircraftMaintenanceCompanyCRUDConfiguration.storeKey +
      '] Load all by post',
    props<{ event: LazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + aircraftMaintenanceCompanyCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + aircraftMaintenanceCompanyCRUDConfiguration.storeKey + '] Create',
    props<{ aircraftMaintenanceCompany: AircraftMaintenanceCompany }>()
  );

  export const update = createAction(
    '[' + aircraftMaintenanceCompanyCRUDConfiguration.storeKey + '] Update',
    props<{ aircraftMaintenanceCompany: AircraftMaintenanceCompany }>()
  );

  export const remove = createAction(
    '[' + aircraftMaintenanceCompanyCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' +
      aircraftMaintenanceCompanyCRUDConfiguration.storeKey +
      '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' +
      aircraftMaintenanceCompanyCRUDConfiguration.storeKey +
      '] Load all by post success',
    props<{
      result: DataResult<AircraftMaintenanceCompany[]>;
      event: LazyLoadEvent;
    }>()
  );

  export const loadSuccess = createAction(
    '[' +
      aircraftMaintenanceCompanyCRUDConfiguration.storeKey +
      '] Load success',
    props<{ aircraftMaintenanceCompany: AircraftMaintenanceCompany }>()
  );

  export const failure = createAction(
    '[' + aircraftMaintenanceCompanyCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' +
      aircraftMaintenanceCompanyCRUDConfiguration.storeKey +
      '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' +
      aircraftMaintenanceCompanyCRUDConfiguration.storeKey +
      '] Clear current'
  );
}
