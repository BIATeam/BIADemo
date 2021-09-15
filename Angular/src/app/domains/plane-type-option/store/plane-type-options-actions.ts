import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export const loadAllPlaneTypeOptions = createAction('[Domain Plane Type Options] Load all');

export const loadAllSuccess = createAction('[Domain Plane Type Options] Load all success', props<{ planesTypes: OptionDto[] }>());
/*
export const load = createAction('[Domain Plane Type Options] Load', props<{ id: number }>());

export const loadSuccess = createAction('[Domain Plane Type Options] Load success', props<{ planeType: PlaneTypeOption }>());
*/
export const failure = createAction('[Domain Plane Type Options] Failure', props<{ error: any }>());


















