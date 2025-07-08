import { createAction, props } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { Notification } from '../model/notification';
import { NotificationListItem } from '../model/notification-list-item';
import { notificationCRUDConfiguration } from '../notification.constants';

export namespace FeatureNotificationsActions {
  export const loadAllByPost = createAction(
    '[' + notificationCRUDConfiguration.storeKey + '] Load all by post',
    props<{ event: TableLazyLoadEvent }>()
  );

  export const load = createAction(
    '[' + notificationCRUDConfiguration.storeKey + '] Load',
    props<{ id: number }>()
  );

  export const setUnread = createAction(
    '[' + notificationCRUDConfiguration.storeKey + '] Set Unread',
    props<{ id: number }>()
  );
  export const create = createAction(
    '[' + notificationCRUDConfiguration.storeKey + '] Create',
    props<{ notification: Notification }>()
  );

  export const update = createAction(
    '[' + notificationCRUDConfiguration.storeKey + '] Update',
    props<{ notification: Notification }>()
  );

  export const remove = createAction(
    '[' + notificationCRUDConfiguration.storeKey + '] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[' + notificationCRUDConfiguration.storeKey + '] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[' + notificationCRUDConfiguration.storeKey + '] Load all by post success',
    props<{
      result: DataResult<NotificationListItem[]>;
      event: TableLazyLoadEvent;
    }>()
  );

  export const loadSuccess = createAction(
    '[' + notificationCRUDConfiguration.storeKey + '] Load success',
    props<{ notification: Notification }>()
  );

  export const failure = createAction(
    '[' + notificationCRUDConfiguration.storeKey + '] Failure',
    props<{ error: any }>()
  );

  export const clearAll = createAction(
    '[' + notificationCRUDConfiguration.storeKey + '] Clear all in state'
  );

  export const clearCurrent = createAction(
    '[' + notificationCRUDConfiguration.storeKey + '] Clear current'
  );
}
