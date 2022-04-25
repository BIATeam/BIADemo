import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { addUnreadNotification, loadAllSuccess, loadSuccess, loadUnreadNotificationIds,
  loadUnreadNotificationIdsSuccess, removeUnreadNotification } from './notifications-actions';
import { Notification } from '../model/notification';

// This adapter will allow is to manipulate notifications (mostly CRUD operations)
export const notificationsAdapter = createEntityAdapter<Notification>({
  selectId: (notification: Notification) => notification.id,
  sortComparer: false
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Notification> {
//   ids: string[] | number[];
//   entities: { [id: string]: Notification };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<Notification> {
  // additional props here
  userNotifications: Notification[] | null;
  loadingUnreadIds: boolean;
  unreadIds: number[];
}

export const INIT_STATE: State = notificationsAdapter.getInitialState({
  // additional props default values here
  userNotifications: null,
  loadingUnreadIds: false,
  unreadIds: []
});

export const notificationReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllSuccess, (state, { notifications }) => notificationsAdapter.setAll(notifications, state)),
  on(loadSuccess, (state, { notification }) => notificationsAdapter.upsertOne(notification, state)),
  on(loadUnreadNotificationIds, (state) => {
    return {
      ...state,
      loadingUnreadIds: true
    };
  }),
  on(loadUnreadNotificationIdsSuccess, (state, { ids }) => {
    return {
      ...state,
      loadingUnreadIds: false,
      unreadIds: ids
    };
  }),
  on(removeUnreadNotification, (state, { id }) => {
    const index = state.unreadIds.indexOf(id, 0);
    const copyState = {
      ...state
    };
    if (index > -1) {
      copyState.unreadIds.splice(index, 1);
    }
    return  copyState;
  }),
  on(addUnreadNotification, (state, { id }) => {
    const copyState = {
      ...state
    };
    copyState.unreadIds.push(id);
    return  copyState;
  }));

export const getNotificationById = (id: number) => (state: State) => state.entities[id];
