import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Plane } from '../model/plane';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export const loadAllByPost = createAction('[Planes] Load all by post (ViewMode)', props<{ event: LazyLoadEvent }>());

export const load = createAction('[Planes] Load (ViewMode)', props<{ id: number }>());

export const create = createAction('[Planes] Create (ViewMode)', props<{ plane: Plane }>());

export const update = createAction('[Planes] Update (ViewMode)', props<{ plane: Plane }>());

export const remove = createAction('[Planes] Remove (ViewMode)', props<{ id: number }>());

export const multiRemove = createAction('[Planes] Multi Remove (ViewMode)', props<{ ids: number[] }>());

export const loadAllByPostSuccess = createAction(
  '[Planes] Load all by post success (ViewMode)',
  props<{ result: DataResult<Plane[]>; event: LazyLoadEvent }>()
);

export const loadSuccess = createAction('[Planes] Load success (ViewMode)', props<{ plane: Plane }>());

export const failure = createAction('[Planes] Failure (ViewMode)', props<{ error: any }>());






