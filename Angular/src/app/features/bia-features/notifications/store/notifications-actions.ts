import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Notification } from '../model/notification';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { NotificationListItem } from '../model/notificationListItem';
export namespace FeatureNotificationsActions {
  export const loadAllByPost = createAction(
    '[Notifications] Load all by post',
    props<{ event: LazyLoadEvent }>()
  );

  export const load = createAction(
    '[Notifications] Load',
    props<{ id: number }>()
  );

  export const setUnread = createAction(
    '[Notifications] Set Unread',
    props<{ id: number }>()
  );

  export const create = createAction(
    '[Notifications] Create',
    props<{ notification: Notification }>()
  );

  export const update = createAction(
    '[Notifications] Update',
    props<{ notification: Notification }>()
  );

  export const remove = createAction(
    '[Notifications] Remove',
    props<{ id: number }>()
  );

  export const multiRemove = createAction(
    '[Notifications] Multi Remove',
    props<{ ids: number[] }>()
  );

  export const loadAllByPostSuccess = createAction(
    '[Notifications] Load all by post success',
    props<{
      result: DataResult<NotificationListItem[]>;
      event: LazyLoadEvent;
    }>()
  );

  export const loadSuccess = createAction(
    '[Notifications] Load success',
    props<{ notification: Notification }>()
  );

  export const failure = createAction(
    '[Notifications] Failure',
    props<{ error: any }>()
  );
}
