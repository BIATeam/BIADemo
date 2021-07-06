import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export const loadAllPlaneTypeOptions = createAction('[Domain PlanesTypes] Load all');

export const loadAllSuccess = createAction('[Domain PlanesTypes] Load all success', props<{ planesTypes: OptionDto[] }>());
/*
export const load = createAction('[Domain PlanesTypes] Load', props<{ id: number }>());

export const loadSuccess = createAction('[Domain PlanesTypes] Load success', props<{ planeType: PlaneTypeOption }>());
*/
export const failure = createAction('[Domain PlanesTypes] Failure', props<{ error: any }>());


















