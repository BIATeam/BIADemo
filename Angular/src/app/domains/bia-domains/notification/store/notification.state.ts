import * as fromNotifications from './notifications-reducer';
import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';

export interface NotificationsState {
  notifications: fromNotifications.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(
  state: NotificationsState | undefined,
  action: Action
) {
  return combineReducers({
    notifications: fromNotifications.notificationReducers,
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getNotificationsState = createFeatureSelector<NotificationsState>(
  'domain-notifications'
);

export const getNotificationsEntitiesState = createSelector(
  getNotificationsState,
  state => state.notifications
);

export const getUserNotifications = createSelector(
  getNotificationsState,
  state => state.notifications?.userNotifications
);

export const getUnreadNotificationCount = createSelector(
  getNotificationsState,
  state => state.notifications.unreadIds.length
);

export const { selectAll: getAllNotifications } =
  fromNotifications.notificationsAdapter.getSelectors(
    getNotificationsEntitiesState
  );

export const getNotificationById = (id: number) =>
  createSelector(
    getNotificationsEntitiesState,
    fromNotifications.getNotificationById(id)
  );
