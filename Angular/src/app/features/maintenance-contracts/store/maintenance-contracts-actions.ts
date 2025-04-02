import { createAction, props } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { maintenanceContractCRUDConfiguration } from '../maintenance-contract.constants';
import { MaintenanceContract } from '../model/maintenance-contract';

export namespace FeatureMaintenanceContractsActions {
  export const loadAllByPost = createAction(
    '[' + maintenanceContractCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + maintenanceContractCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + maintenanceContractCRUDConfiguration.storeKey + '] Create',
    props<{ maintenanceContract: MaintenanceContract }>()
  );

  export const update = createAction(
    '[' + maintenanceContractCRUDConfiguration.storeKey + '] Update',
    props<{ maintenanceContract: MaintenanceContract }>()
  );

  export const save = createAction(
    '[' + maintenanceContractCRUDConfiguration.storeKey + '] Save',
    props<{ maintenanceContracts: MaintenanceContract[] }>()
  );

  export const remove = createAction(
    '[' + maintenanceContractCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + maintenanceContractCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' +
      maintenanceContractCRUDConfiguration.storeKey +
      '] Load all by post success',
    props<{
      result: DataResult<MaintenanceContract[]>;
      event: TableLazyLoadEvent;
    }>()
  );

  export const loadSuccess = createAction(
    '[' + maintenanceContractCRUDConfiguration.storeKey + '] Load success',
    props<{ maintenanceContract: MaintenanceContract }>()
  );

  export const failure = createAction(
    '[' + maintenanceContractCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + maintenanceContractCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + maintenanceContractCRUDConfiguration.storeKey + '] Clear current'
  );
}
