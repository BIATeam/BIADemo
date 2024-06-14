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

export const getNotificationsState =
  createFeatureSelector<NotificationsState>('notifications');

export const getNotificationsEntitiesState = createSelector(
  getNotificationsState,
  state => state.notifications
);

export const getNotificationsTotalCount = createSelector(
  getNotificationsEntitiesState,
  state => state.totalCount
);

export const getCurrentNotification = createSelector(
  getNotificationsEntitiesState,
  state => state.currentNotification
);

export const getLastLazyLoadEvent = createSelector(
  getNotificationsEntitiesState,
  state => state.lastLazyLoadEvent
);

export const getNotificationLoadingGet = createSelector(
  getNotificationsEntitiesState,
  state => state.loadingGet
);

export const getNotificationLoadingGetAll = createSelector(
  getNotificationsEntitiesState,
  state => state.loadingGetAll
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
