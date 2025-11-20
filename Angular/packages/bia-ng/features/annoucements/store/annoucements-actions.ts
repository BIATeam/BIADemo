import { createAction, props } from '@ngrx/store';
import { HistoricalEntryDto } from 'packages/bia-ng/models/dto/historical-entry-dto';
import { DataResult } from 'packages/bia-ng/models/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { annoucementCRUDConfiguration } from '../annoucement.constants';
import { Annoucement } from '../model/annoucement';

export namespace FeatureAnnoucementsActions {
  export const loadAllByPost = createAction(
    '[' + annoucementCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + annoucementCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + annoucementCRUDConfiguration.storeKey + '] Create',
    props<{ annoucement: Annoucement }>()
  );

  export const update = createAction(
    '[' + annoucementCRUDConfiguration.storeKey + '] Update',
    props<{ annoucement: Annoucement }>()
  );

  export const remove = createAction(
    '[' + annoucementCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + annoucementCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' + annoucementCRUDConfiguration.storeKey + '] Load all by post success',
    props<{ result: DataResult<Annoucement[]>; event: TableLazyLoadEvent }>()
  );

  export const loadSuccess = createAction(
    '[' + annoucementCRUDConfiguration.storeKey + '] Load success',
    props<{ annoucement: Annoucement }>()
  );

  export const failure = createAction(
    '[' + annoucementCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + annoucementCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + annoucementCRUDConfiguration.storeKey + '] Clear current'
  );

  export const loadHistorical = createAction(
    '[' + annoucementCRUDConfiguration.storeKey + '] Load historical',
    props<{ id: number }>()
  );

  export const loadHistoricalSuccess = createAction(
    '[' + annoucementCRUDConfiguration.storeKey + '] Load historical success',
    props<{ historical: HistoricalEntryDto[] }>()
  );
}
