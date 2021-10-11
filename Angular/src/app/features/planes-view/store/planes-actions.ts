import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Plane } from '../model/plane';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export const loadAllByPost = createAction('[Planes ViewMode] Load all by post', props<{ event: LazyLoadEvent }>());

export const load = createAction('[Planes ViewMode] Load', props<{ id: number }>());

export const create = createAction('[Planes ViewMode] Create', props<{ plane: Plane }>());

export const update = createAction('[Planes ViewMode] Update', props<{ plane: Plane }>());

export const remove = createAction('[Planes ViewMode] Remove', props<{ id: number }>());

export const multiRemove = createAction('[Planes ViewMode] Multi Remove', props<{ ids: number[] }>());

export const loadAllByPostSuccess = createAction(
  '[Planes ViewMode] Load all by post success',
  props<{ result: DataResult<Plane[]>; event: LazyLoadEvent }>()
);

export const loadSuccess = createAction('[Planes ViewMode] Load success', props<{ plane: Plane }>());

export const failure = createAction('[Planes ViewMode] Failure', props<{ error: any }>());






