import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { BaseDto } from '../../../model/base-dto';
import { storeKey } from '../crud-item.constants';

export class CrudItemsActions<CrudItem extends BaseDto>
{
  loadAllByPost = createAction('[' + storeKey +'] Load all by post', props<{ event: LazyLoadEvent }>());

  load = createAction('[' + storeKey +'] Load', props<{ id: number }>());
  
  create = createAction('[' + storeKey +'] Create', props<{ crudItem: CrudItem }>());
  
  update = createAction('[' + storeKey +'] Update', props<{ crudItem: CrudItem }>());
  
  remove = createAction('[' + storeKey +'] Remove', props<{ id: number }>());
  
  multiRemove = createAction('[' + storeKey +'] Multi Remove', props<{ ids: number[] }>());
  
  loadAllByPostSuccess = createAction(
    '[' + storeKey +'] Load all by post success',
    props<{ result: DataResult<CrudItem[]>; event: LazyLoadEvent }>()
  );
  
  loadSuccess = createAction('[' + storeKey +'] Load success', props<{ crudItem: CrudItem }>());
  
  failure = createAction('[' + storeKey +'] Failure', props<{ error: any }>());
}