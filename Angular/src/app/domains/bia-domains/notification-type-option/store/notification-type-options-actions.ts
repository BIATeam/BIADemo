import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export namespace DomainNotificationTypeOptionsActions {
  export const loadAll = createAction(
    '[Domain Notification Type Options] Load all'
  );

  export const loadAllSuccess = createAction(
    '[Domain Notification Type Options] Load all success',
    props<{ notificationTypes: OptionDto[] }>()
  );

  export const failure = createAction(
    '[Domain Notification Type Options] Failure',
    props<{ error: any }>()
  );
}
