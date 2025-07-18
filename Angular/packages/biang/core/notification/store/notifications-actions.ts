import { createAction, props } from '@ngrx/store';
import { Notification } from '../model/notification';

export namespace DomainNotificationsActions {
  export const loadAll = createAction('[Domain Notifications] Load all');

  export const loadAllSuccess = createAction(
    '[Domain Notifications] Load all success',
    props<{ notifications: Notification[] }>()
  );

  export const load = createAction(
    '[Domain Notifications] Load',
    props<{ id: number }>()
  );

  export const loadSuccess = createAction(
    '[Domain Notifications] Load success',
    props<{ notification: Notification }>()
  );

  export const failure = createAction(
    '[Domain Notifications] Failure',
    props<{ error: any }>()
  );

  export const loadUnreadNotificationIds = createAction(
    '[Domain Notifications] Load unread notification ids'
  );

  export const loadUnreadNotificationIdsSuccess = createAction(
    '[Domain Notifications] Load unread notification ids success',
    props<{ ids: number[] }>()
  );

  export const removeUnreadNotification = createAction(
    '[Domain Notifications] Read notification id',
    props<{ id: number }>()
  );

  export const addUnreadNotification = createAction(
    '[Domain Notifications] Add unread notification id',
    props<{ id: number }>()
  );

  export const setAsRead = createAction(
    '[Domain Notifications] Set As Read',
    props<{ id: number }>()
  );

  export const setAsReadSuccess = createAction(
    '[Domain Notifications] Set As Read Success'
  );
}
