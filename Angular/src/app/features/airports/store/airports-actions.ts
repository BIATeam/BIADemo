import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Airport } from '../model/airport';
import { AirportCRUDConfiguration } from '../airport.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export namespace FeatureAirportsActions
{
  export const loadAllByPost = createAction('[' + AirportCRUDConfiguration.storeKey +'] Load all by post', props<{ event: LazyLoadEvent }>());

  export const load = createAction('[' + AirportCRUDConfiguration.storeKey +'] Load', props<{ id: number }>());
  
  export const create = createAction('[' + AirportCRUDConfiguration.storeKey +'] Create', props<{ airport: Airport }>());
  
  export const update = createAction('[' + AirportCRUDConfiguration.storeKey +'] Update', props<{ airport: Airport }>());
  
  export const remove = createAction('[' + AirportCRUDConfiguration.storeKey +'] Remove', props<{ id: number }>());
  
  export const multiRemove = createAction('[' + AirportCRUDConfiguration.storeKey +'] Multi Remove', props<{ ids: number[] }>());
  
  export const loadAllByPostSuccess = createAction(
    '[' + AirportCRUDConfiguration.storeKey +'] Load all by post success',
    props<{ result: DataResult<Airport[]>; event: LazyLoadEvent }>()
  );
  
  export const loadSuccess = createAction('[' + AirportCRUDConfiguration.storeKey +'] Load success', props<{ airport: Airport }>());
  
  export const failure = createAction('[' + AirportCRUDConfiguration.storeKey +'] Failure', props<{ error: any }>());
}