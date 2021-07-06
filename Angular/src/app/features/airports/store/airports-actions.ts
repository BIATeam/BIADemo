import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Airport } from '../model/airport';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export const loadAllByPost = createAction('[Airports] Load all by post', props<{ event: LazyLoadEvent }>());

export const load = createAction('[Airports] Load', props<{ id: number }>());

export const create = createAction('[Airports] Create', props<{ airport: Airport }>());

export const update = createAction('[Airports] Update', props<{ airport: Airport }>());

export const remove = createAction('[Airports] Remove', props<{ id: number }>());

export const multiRemove = createAction('[Airports] Multi Remove', props<{ ids: number[] }>());

export const loadAllByPostSuccess = createAction(
  '[Airports] Load all by post success',
  props<{ result: DataResult<Airport[]>; event: LazyLoadEvent }>()
);

export const loadSuccess = createAction('[Airports] Load success', props<{ airport: Airport }>());

export const failure = createAction('[Airports] Failure', props<{ error: any }>());

export const openDialogEdit = createAction('[Airports] Open dialog edit');

export const closeDialogEdit = createAction('[Airports] Close dialog edit');

export const openDialogNew = createAction('[Airports] Open dialog new');

export const closeDialogNew = createAction('[Airports] Close dialog new');
