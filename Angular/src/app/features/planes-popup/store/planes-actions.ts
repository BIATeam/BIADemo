import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Plane } from '../model/plane';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export const loadAllByPost = createAction('[Planes Popup] Load all by post', props<{ event: LazyLoadEvent }>());

export const load = createAction('[Planes Popup] Load', props<{ id: number }>());

export const create = createAction('[Planes Popup] Create', props<{ plane: Plane }>());

export const update = createAction('[Planes Popup] Update', props<{ plane: Plane }>());

export const remove = createAction('[Planes Popup] Remove', props<{ id: number }>());

export const multiRemove = createAction('[Planes Popup] Multi Remove', props<{ ids: number[] }>());

export const loadAllByPostSuccess = createAction(
  '[Planes Popup] Load all by post success',
  props<{ result: DataResult<Plane[]>; event: LazyLoadEvent }>()
);

export const loadSuccess = createAction('[Planes Popup] Load success', props<{ plane: Plane }>());

export const failure = createAction('[Planes Popup] Failure', props<{ error: any }>());

export const openDialogEdit = createAction('[Planes Popup] Open dialog edit');

export const closeDialogEdit = createAction('[Planes Popup] Close dialog edit');

export const openDialogNew = createAction('[Planes Popup] Open dialog new');

export const closeDialogNew = createAction('[Planes Popup] Close dialog new');
