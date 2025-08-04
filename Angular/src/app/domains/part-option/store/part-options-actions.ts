import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'bia-ng/models';
import { storeKey } from '../part-option.contants';

export namespace DomainPartOptionsActions {
  export const loadAll = createAction('[' + storeKey + '] Load all');

  export const loadAllSuccess = createAction(
    '[' + storeKey + '] Load all success',
    props<{ parts: OptionDto[] }>()
  );

  export const failure = createAction(
    '[' + storeKey + '] Failure',
    props<{ error: any }>()
  );
}
