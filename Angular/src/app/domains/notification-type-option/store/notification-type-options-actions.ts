import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export const loadAllNotificationTypeOptions = createAction('[Domain NotificationTypes] Load all');

export const loadAllSuccess = createAction('[Domain NotificationTypes] Load all success', props<{ notificationTypes: OptionDto[] }>());
/*
export const load = createAction('[Domain NotificationTypes] Load', props<{ id: number }>());

export const loadSuccess = createAction('[Domain NotificationTypes] Load success', props<{ notificationType: NotificationTypeOption }>());
*/
export const failure = createAction('[Domain NotificationTypes] Failure', props<{ error: any }>());


















