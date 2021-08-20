import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { Notification } from 'src/app/features/notifications/model/notification';

export const loadAllByPost = createAction('[Notifications] Load all by post', props<{ event: LazyLoadEvent }>());

export const load = createAction('[Notifications] Load', props<{ id: number }>());

export const update = createAction('[Notifications] Update', props<{ notification: Notification }>());

export const remove = createAction('[Notifications] Remove', props<{ id: number }>());

export const multiRemove = createAction('[Notifications] Multi Remove', props<{ ids: number[] }>());

export const loadAllSuccess = createAction('[Notifications] Load all success', props<{ notifications: Notification[] }>());

export const loadAllByPostSuccess = createAction(
  '[Notifications] Load all by post success',
  props<{ result: DataResult<Notification[]>; event: LazyLoadEvent }>()
);

export const loadSuccess = createAction('[Notifications] Load success', props<{ notification: Notification }>());

export const updateSuccess = createAction('[Notifications] Update success', props<{ notification: Notification }>());

export const removeSuccess = createAction('[Notifications] Remove success', props<{ id: number }>());

export const failure = createAction('[Notifications] Failure', props<{ err: { concern: 'CREATE' | 'PATCH'; error: any } }>());

export const callWorkerWithNotification = createAction('[Notifications] Call worker with notification');
