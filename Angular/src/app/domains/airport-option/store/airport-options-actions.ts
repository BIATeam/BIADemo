import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'biang/models';
import { storeKey } from '../airport-option.contants';

export namespace DomainAirportOptionsActions {
  export const loadAll = createAction('[' + storeKey + '] Load all');

  export const loadAllSuccess = createAction(
    '[' + storeKey + '] Load all success',
    props<{ airports: OptionDto[] }>()
  );

  export const failure = createAction(
    '[' + storeKey + '] Failure',
    props<{ error: any }>()
  );
}
