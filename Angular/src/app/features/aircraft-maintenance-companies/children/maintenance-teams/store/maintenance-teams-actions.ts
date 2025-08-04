import { createAction, props } from '@ngrx/store';
import { DataResult } from 'packages/bia-ng/models/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { maintenanceTeamCRUDConfiguration } from '../maintenance-team.constants';
import { MaintenanceTeam } from '../model/maintenance-team';

export namespace FeatureMaintenanceTeamsActions {
  export const loadAllByPost = createAction(
    '[' + maintenanceTeamCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + maintenanceTeamCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + maintenanceTeamCRUDConfiguration.storeKey + '] Create',
    props<{ maintenanceTeam: MaintenanceTeam }>()
  );

  export const update = createAction(
    '[' + maintenanceTeamCRUDConfiguration.storeKey + '] Update',
    props<{ maintenanceTeam: MaintenanceTeam }>()
  );

  export const remove = createAction(
    '[' + maintenanceTeamCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + maintenanceTeamCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' +
      maintenanceTeamCRUDConfiguration.storeKey +
      '] Load all by post success',
    props<{
      result: DataResult<MaintenanceTeam[]>;
      event: TableLazyLoadEvent;
    }>()
  );

  export const loadSuccess = createAction(
    '[' + maintenanceTeamCRUDConfiguration.storeKey + '] Load success',
    props<{ maintenanceTeam: MaintenanceTeam }>()
  );

  export const failure = createAction(
    '[' + maintenanceTeamCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + maintenanceTeamCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + maintenanceTeamCRUDConfiguration.storeKey + '] Clear current'
  );

  export const updateFixedStatus = createAction(
    '[' + maintenanceTeamCRUDConfiguration.storeKey + '] Update fixed status',
    props<{ id: number; isFixed: boolean }>()
  );
}
