import { createAction, props } from '@ngrx/store';
import { HistoricalEntryDto } from 'packages/bia-ng/models/dto/historical-entry-dto';
import { DataResult } from 'packages/bia-ng/models/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { bannerMessageCRUDConfiguration } from '../banner-message.constants';
import { BannerMessage } from '../model/banner-message';

export namespace FeatureBannerMessagesActions {
  export const loadAllByPost = createAction(
    '[' + bannerMessageCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + bannerMessageCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + bannerMessageCRUDConfiguration.storeKey + '] Create',
    props<{ bannerMessage: BannerMessage }>()
  );

  export const update = createAction(
    '[' + bannerMessageCRUDConfiguration.storeKey + '] Update',
    props<{ bannerMessage: BannerMessage }>()
  );

  export const remove = createAction(
    '[' + bannerMessageCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + bannerMessageCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' +
      bannerMessageCRUDConfiguration.storeKey +
      '] Load all by post success',
    props<{ result: DataResult<BannerMessage[]>; event: TableLazyLoadEvent }>()
  );

  export const loadSuccess = createAction(
    '[' + bannerMessageCRUDConfiguration.storeKey + '] Load success',
    props<{ bannerMessage: BannerMessage }>()
  );

  export const failure = createAction(
    '[' + bannerMessageCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + bannerMessageCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + bannerMessageCRUDConfiguration.storeKey + '] Clear current'
  );

  export const loadHistorical = createAction(
    '[' + bannerMessageCRUDConfiguration.storeKey + '] Load historical',
    props<{ id: number }>()
  );

  export const loadHistoricalSuccess = createAction(
    '[' + bannerMessageCRUDConfiguration.storeKey + '] Load historical success',
    props<{ historical: HistoricalEntryDto[] }>()
  );
}
