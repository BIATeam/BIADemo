import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'biang/models';
import { storeKey } from '../team-option.contants';

export namespace DomainTeamOptionsActions {
  export const loadAll = createAction('[' + storeKey + '] Load all');

  export const loadAllSuccess = createAction(
    '[' + storeKey + '] Load all success',
    props<{ teams: OptionDto[] }>()
  );

  export const failure = createAction(
    '[' + storeKey + '] Failure',
    props<{ error: any }>()
  );
}
