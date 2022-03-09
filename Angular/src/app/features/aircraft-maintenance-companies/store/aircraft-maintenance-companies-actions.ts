import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { AircraftMaintenanceCompany } from '../model/aircraft-maintenance-company';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { storeKey } from '../aircraft-maintenance-company.constants';

export namespace FeatureAircraftMaintenanceCompaniesActions
{
  export const loadAllByPost = createAction('[' + storeKey +'] Load all by post', props<{ event: LazyLoadEvent }>());

  export const load = createAction('[' + storeKey +'] Load', props<{ id: number }>());
  
  export const create = createAction('[' + storeKey +'] Create', props<{ aircraftMaintenanceCompany: AircraftMaintenanceCompany }>());
  
  export const update = createAction('[' + storeKey +'] Update', props<{ aircraftMaintenanceCompany: AircraftMaintenanceCompany }>());
  
  export const remove = createAction('[' + storeKey +'] Remove', props<{ id: number }>());
  
  export const multiRemove = createAction('[' + storeKey +'] Multi Remove', props<{ ids: number[] }>());
  
  export const loadAllByPostSuccess = createAction(
    '[' + storeKey +'] Load all by post success',
    props<{ result: DataResult<AircraftMaintenanceCompany[]>; event: LazyLoadEvent }>()
  );
  
  export const loadSuccess = createAction('[' + storeKey +'] Load success', props<{ aircraftMaintenanceCompany: AircraftMaintenanceCompany }>());
  
  export const failure = createAction('[' + storeKey +'] Failure', props<{ error: any }>());
}