import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'biang/models';
import { storeKey } from '../plane-type-option.constants';

export namespace DomainPlaneTypeOptionsActions {
  export const loadAll = createAction('[' + storeKey + '] Load all');

  export const loadAllSuccess = createAction(
    '[' + storeKey + '] Load all success',
    props<{ planeTypes: OptionDto[] }>()
  );

  export const failure = createAction(
    '[' + storeKey + '] Failure',
    props<{ error: any }>()
  );
}
