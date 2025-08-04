import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'bia-ng/models';
import { storeKey } from '../plane-option.contants';

export namespace DomainPlaneOptionsActions {
  export const loadAll = createAction('[' + storeKey + '] Load all');

  export const loadAllSuccess = createAction(
    '[' + storeKey + '] Load all success',
    props<{ planes: OptionDto[] }>()
  );

  export const failure = createAction(
    '[' + storeKey + '] Failure',
    props<{ error: any }>()
  );
}
