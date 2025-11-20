import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import { storeKey } from '../annoucement-type-option.constants';

export namespace DomainAnnoucementTypeOptionsActions {
  export const loadAll = createAction('[' + storeKey + '] Load all');

  export const loadAllSuccess = createAction(
    '[' + storeKey + '] Load all success',
    props<{ annoucementTypes: OptionDto[] }>()
  );

  export const failure = createAction(
    '[' + storeKey + '] Failure',
    props<{ error: any }>()
  );
}
