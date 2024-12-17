import {
  Action,
  combineReducers,
  createFeatureSelector,
  createSelector,
} from '@ngrx/store';
import * as fromNotificationTypeOptions from './notification-type-options-reducer';

export interface NotificationTypeOptionsState {
  notificationTypeOptions: fromNotificationTypeOptions.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(
  state: NotificationTypeOptionsState | undefined,
  action: Action
) {
  return combineReducers({
    notificationTypeOptions:
      fromNotificationTypeOptions.notificationTypeOptionReducers,
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getNotificationTypesState =
  createFeatureSelector<NotificationTypeOptionsState>(
    'domain-notification-type-options'
  );

export const getNotificationTypeOptionsEntitiesState = createSelector(
  getNotificationTypesState,
  state => state.notificationTypeOptions
);

export const { selectAll: getAllNotificationTypeOptions } =
  fromNotificationTypeOptions.notificationTypeOptionsAdapter.getSelectors(
    getNotificationTypeOptionsEntitiesState
  );

export const getNotificationTypeOptionById = (id: number) =>
  createSelector(
    getNotificationTypeOptionsEntitiesState,
    fromNotificationTypeOptions.getNotificationTypeOptionById(id)
  );
