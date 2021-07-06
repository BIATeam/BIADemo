import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Plane } from '../model/plane';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export const loadAllByPost = createAction('[Planes] Load all by post (CalcMode)', props<{ event: LazyLoadEvent }>());

export const load = createAction('[Planes] Load (CalcMode)', props<{ id: number }>());

export const create = createAction('[Planes] Create (CalcMode)', props<{ plane: Plane }>());

export const update = createAction('[Planes] Update (CalcMode)', props<{ plane: Plane }>());

export const remove = createAction('[Planes] Remove (CalcMode)', props<{ id: number }>());

export const multiRemove = createAction('[Planes] Multi Remove (CalcMode)', props<{ ids: number[] }>());

export const loadAllByPostSuccess = createAction(
  '[Planes] Load all by post success (CalcMode)',
  props<{ result: DataResult<Plane[]>; event: LazyLoadEvent }>()
);

export const loadSuccess = createAction('[Planes] Load success (CalcMode)', props<{ plane: Plane }>());

export const failure = createAction('[Planes] Failure (CalcMode)', props<{ error: any }>());






