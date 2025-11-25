import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import { storeKey } from '../team-option.contants';

export namespace DomainTeamOptionsActions {
  export const loadAll = createAction('[' + storeKey + '] Load all');

  export const loadAllSuccess = createAction(
    '[' + storeKey + '] Load all success',
    props<{ teams: OptionDto[] }>()
  );

  export const loadAllAssignViewAllowed = createAction(
    '[' + storeKey + '] Load all assign view allowed'
  );

  export const loadAllAssignViewAllowedSuccess = createAction(
    '[' + storeKey + '] Load all assign view allowed success',
    props<{ teams: OptionDto[] }>()
  );

  export const failure = createAction(
    '[' + storeKey + '] Failure',
    props<{ error: any }>()
  );
}
