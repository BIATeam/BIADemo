import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export const loadAllAirportOptions = createAction('[Domain Airport Options] Load all');

export const loadAllSuccess = createAction('[Domain Airport Options] Load all success', props<{ airports: OptionDto[] }>());
/*
export const load = createAction('[Domain Airports] Load', props<{ id: number }>());

export const loadSuccess = createAction('[Domain Airports] Load success', props<{ airport: AirportOption }>());
*/
export const failure = createAction('[Domain Airport Options] Failure', props<{ error: any }>());


















