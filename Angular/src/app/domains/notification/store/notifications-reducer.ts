import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { loadAllSuccess, loadSuccess, loadUnreadNotificationCount, loadUnreadNotificationCountSuccess } from './notifications-actions';
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
  loadingUnreadCount: boolean;
  unreadCount: number;
}

export const INIT_STATE: State = notificationsAdapter.getInitialState({
  // additional props default values here
  userNotifications: null,
  loadingUnreadCount: false,
  unreadCount: 0
});

export const notificationReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllSuccess, (state, { notifications }) => notificationsAdapter.setAll(notifications, state)),
  on(loadSuccess, (state, { notification }) => notificationsAdapter.upsertOne(notification, state)),
  on(loadUnreadNotificationCount, (state) => {
    return {
      ...state,
      loadingUnreadCount: true
    };
  }),
  on(loadUnreadNotificationCountSuccess, (state, { count }) => {
    return {
      ...state,
      loadingUnreadCount: false,
      unreadCount: count
    };
  })
);

export const getNotificationById = (id: number) => (state: State) => state.entities[id];
