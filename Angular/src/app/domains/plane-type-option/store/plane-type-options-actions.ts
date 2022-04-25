import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { storeKey } from '../plane-type-option.contants';

export namespace DomainPlaneTypeOptionsActions
{ 
    export const loadAll = createAction('[' + storeKey + '] Load all');

    export const loadAllSuccess = createAction('[' + storeKey + '] Load all success', props<{ planesTypes: OptionDto[] }>());

    export const failure = createAction('[' + storeKey + '] Failure', props<{ error: any }>());
}
