import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import { storeKey } from '../announcement-type-option.constants';

export namespace DomainAnnouncementTypeOptionsActions {
  export const loadAll = createAction('[' + storeKey + '] Load all');

  export const loadAllSuccess = createAction(
    '[' + storeKey + '] Load all success',
    props<{ announcementTypes: OptionDto[] }>()
  );

  export const failure = createAction(
    '[' + storeKey + '] Failure',
    props<{ error: any }>()
  );
}
