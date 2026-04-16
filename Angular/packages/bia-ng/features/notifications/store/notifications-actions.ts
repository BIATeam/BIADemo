import { createAction, props } from '@ngrx/store';
import { DataResult } from 'packages/bia-ng/models/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { Notification } from '../model/notification';
import { NotificationListItem } from '../model/notification-list-item';
import { notificationCRUDConfiguration } from '../notification.constants';

const STORE_KEY = notificationCRUDConfiguration.storeKey;

export namespace FeatureNotificationsActions {
  export const loadAllByPost = createAction(
    '[' + STORE_KEY + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + STORE_KEY + '] Load',
    props<{ id: number }>()
  );

  export const setUnread = createAction(
    '[' + STORE_KEY + '] Set Unread',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[' + STORE_KEY + '] Create',
    props<{ notification: Notification }>()
  );

  export const update = createAction(
    '[' + STORE_KEY + '] Update',
    props<{ notification: Notification }>()
  );

  export const remove = createAction(
    '[' + STORE_KEY + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + STORE_KEY + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' + STORE_KEY + '] Load all by post success',
    props<{
      result: DataResult<NotificationListItem[]>;
      event: TableLazyLoadEvent;
    }>()
  );

  export const loadSuccess = createAction(
    '[' + STORE_KEY + '] Load success',
    props<{ notification: Notification }>()
  );

  export const failure = createAction(
    '[' + STORE_KEY + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + STORE_KEY + '] Clear all in state'
  );

  export const clearCurrent = createAction('[' + STORE_KEY + '] Clear current');
}
