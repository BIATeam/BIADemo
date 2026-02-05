import { createAction, props } from '@ngrx/store';
import {
  Announcement,
  DataResult,
  HistoricalEntryDto,
} from 'packages/bia-ng/models/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { announcementCRUDConfiguration } from '../announcement.constants';

export namespace FeatureAnnouncementsActions {
  export const loadAllByPost = createAction(
    '[' + announcementCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + announcementCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + announcementCRUDConfiguration.storeKey + '] Create',
    props<{ announcement: Announcement }>()
  );

  export const update = createAction(
    '[' + announcementCRUDConfiguration.storeKey + '] Update',
    props<{ announcement: Announcement }>()
  );

  export const remove = createAction(
    '[' + announcementCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + announcementCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' + announcementCRUDConfiguration.storeKey + '] Load all by post success',
    props<{ result: DataResult<Announcement[]>; event: TableLazyLoadEvent }>()
  );

  export const loadSuccess = createAction(
    '[' + announcementCRUDConfiguration.storeKey + '] Load success',
    props<{ announcement: Announcement }>()
  );

  export const failure = createAction(
    '[' + announcementCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + announcementCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + announcementCRUDConfiguration.storeKey + '] Clear current'
  );

  export const loadHistorical = createAction(
    '[' + announcementCRUDConfiguration.storeKey + '] Load historical',
    props<{ id: number }>()
  );

  export const loadHistoricalSuccess = createAction(
    '[' + announcementCRUDConfiguration.storeKey + '] Load historical success',
    props<{ historical: HistoricalEntryDto[] }>()
  );
}
