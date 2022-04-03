import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { MaintenanceTeam } from '../model/maintenance-team';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { storeKey } from '../maintenance-team.constants';

export namespace FeatureMaintenanceTeamsActions
{
  export const loadAllByPost = createAction('[' + storeKey +'] Load all by post', props<{ event: LazyLoadEvent }>());

  export const load = createAction('[' + storeKey +'] Load', props<{ id: number }>());
  
  export const create = createAction('[' + storeKey +'] Create', props<{ maintenanceTeam: MaintenanceTeam }>());
  
  export const update = createAction('[' + storeKey +'] Update', props<{ maintenanceTeam: MaintenanceTeam }>());
  
  export const remove = createAction('[' + storeKey +'] Remove', props<{ id: number }>());
  
  export const multiRemove = createAction('[' + storeKey +'] Multi Remove', props<{ ids: number[] }>());
  
  export const loadAllByPostSuccess = createAction(
    '[' + storeKey +'] Load all by post success',
    props<{ result: DataResult<MaintenanceTeam[]>; event: LazyLoadEvent }>()
  );
  
  export const loadSuccess = createAction('[' + storeKey +'] Load success', props<{ maintenanceTeam: MaintenanceTeam }>());
  
  export const failure = createAction('[' + storeKey +'] Failure', props<{ error: any }>());
}