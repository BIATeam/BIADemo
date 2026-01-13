import { OptionDto } from '@bia-team/bia-ng/models';
import { createAction, props } from '@ngrx/store';
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
