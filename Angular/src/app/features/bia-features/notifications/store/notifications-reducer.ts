import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { clone } from 'src/app/shared/bia-shared/utils';
import { Notification } from '../model/notification';
import { NotificationListItem } from '../model/notification-list-item';
import { FeatureNotificationsActions } from './notifications-actions';

// This adapter will allow is to manipulate notifications (mostly CRUD operations)
export const notificationsAdapter = createEntityAdapter<NotificationListItem>({
  selectId: (notification: NotificationListItem) => notification.id,
  sortComparer: false,
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

export interface State extends EntityState<NotificationListItem> {
  // additional props here
  totalCount: number;
  currentNotification: Notification;
  lastLazyLoadEvent: TableLazyLoadEvent;
  loadingGet: boolean;
  loadingGetAll: boolean;
}

export const INIT_STATE: State = notificationsAdapter.getInitialState({
  // additional props default values here
  totalCount: 0,
  currentNotification: <Notification>{},
  lastLazyLoadEvent: <TableLazyLoadEvent>{},
  loadingGet: false,
  loadingGetAll: false,
});

export const notificationReducers = createReducer<State>(
  INIT_STATE,
  on(FeatureNotificationsActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureNotificationsActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(
    FeatureNotificationsActions.loadAllByPostSuccess,
    (state, { result, event }) => {
      const stateUpdated = notificationsAdapter.setAll(result.data, state);
      stateUpdated.totalCount = result.totalCount;
      stateUpdated.lastLazyLoadEvent = event;
      stateUpdated.loadingGetAll = false;
      return stateUpdated;
    }
  ),
  on(FeatureNotificationsActions.loadSuccess, (state, { notification }) => {
    const notif = clone(notification);
    try {
      notif.data = notification.jData
        ? JSON.parse(notification.jData)
        : { route: null, display: '', teams: null };
    } catch {
      notif.data = { route: null, display: '', teams: null };
    }
    return { ...state, currentNotification: notif, loadingGet: false };
  }),
  on(FeatureNotificationsActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getNotificationById = (id: number) => (state: State) =>
  state.entities[id];
