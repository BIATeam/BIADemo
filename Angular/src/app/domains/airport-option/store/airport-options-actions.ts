import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { storeKey } from '../airport-option.contants';

export const loadAllAirportOptions = createAction('[' + storeKey + '] Load all');

export const loadAllSuccess = createAction('[' + storeKey + '] Load all success', props<{ airports: OptionDto[] }>());

export const failure = createAction('[' + storeKey + '] Failure', props<{ error: any }>());
