import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { MaintenanceTeam } from '../model/maintenance-team';
import { MaintenanceTeamCRUDConfiguration } from '../maintenance-team.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export namespace FeatureMaintenanceTeamsActions
{
  export const loadAllByPost = createAction('[' + MaintenanceTeamCRUDConfiguration.storeKey +'] Load all by post', props<{ event: LazyLoadEvent }>());

  export const load = createAction('[' + MaintenanceTeamCRUDConfiguration.storeKey +'] Load', props<{ id: number }>());
  
  export const create = createAction('[' + MaintenanceTeamCRUDConfiguration.storeKey +'] Create', props<{ maintenanceTeam: MaintenanceTeam }>());
  
  export const update = createAction('[' + MaintenanceTeamCRUDConfiguration.storeKey +'] Update', props<{ maintenanceTeam: MaintenanceTeam }>());
  
  export const remove = createAction('[' + MaintenanceTeamCRUDConfiguration.storeKey +'] Remove', props<{ id: number }>());
  
  export const multiRemove = createAction('[' + MaintenanceTeamCRUDConfiguration.storeKey +'] Multi Remove', props<{ ids: number[] }>());
  
  export const loadAllByPostSuccess = createAction(
    '[' + MaintenanceTeamCRUDConfiguration.storeKey +'] Load all by post success',
    props<{ result: DataResult<MaintenanceTeam[]>; event: LazyLoadEvent }>()
  );
  
  export const loadSuccess = createAction('[' + MaintenanceTeamCRUDConfiguration.storeKey +'] Load success', props<{ maintenanceTeam: MaintenanceTeam }>());
  
  export const failure = createAction('[' + MaintenanceTeamCRUDConfiguration.storeKey +'] Failure', props<{ error: any }>());
  
  export const clearAll = createAction('[' + MaintenanceTeamCRUDConfiguration.storeKey +'] Clear all in state');
  
  export const clearCurrent = createAction('[' + MaintenanceTeamCRUDConfiguration.storeKey +'] Clear current');
}