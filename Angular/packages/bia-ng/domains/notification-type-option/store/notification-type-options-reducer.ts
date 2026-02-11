import { OptionDto } from '@bia-team/bia-ng/models';
import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { DomainNotificationTypeOptionsActions } from './notification-type-options-actions';

// This adapter will allow is to manipulate notificationTypes (mostly CRUD operations)
export const notificationTypeOptionsAdapter = createEntityAdapter<OptionDto>({
  selectId: (notificationType: OptionDto) => notificationType.id,
  sortComparer: false,
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<NotificationType> {
//   ids: string[] | number[];
//   entities: { [id: string]: NotificationType };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export type NotificationTypeState = EntityState<OptionDto>;

export const INIT_NOTIFICATIONTYPE_STATE: NotificationTypeState =
  notificationTypeOptionsAdapter.getInitialState({
    // additional props default values here
  });

export const notificationTypeOptionReducers =
  createReducer<NotificationTypeState>(
    INIT_NOTIFICATIONTYPE_STATE,
    on(
      DomainNotificationTypeOptionsActions.loadAllSuccess,
      (state, { notificationTypes }) =>
        notificationTypeOptionsAdapter.setAll(notificationTypes, state)
    )
    // on(loadSuccess, (state, { notificationType }) => notificationTypeOptionsAdapter.upsertOne(notificationType, state))
  );

export const getNotificationTypeOptionById =
  (id: number) => (state: NotificationTypeState) =>
    state.entities[id];
