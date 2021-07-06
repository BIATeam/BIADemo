import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Plane } from '../model/plane';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export const loadAllByPost = createAction('[Planes] Load all by post (SignalR)', props<{ event: LazyLoadEvent }>());

export const load = createAction('[Planes] Load (SignalR)', props<{ id: number }>());

export const create = createAction('[Planes] Create (SignalR)', props<{ plane: Plane }>());

export const update = createAction('[Planes] Update (SignalR)', props<{ plane: Plane }>());

export const remove = createAction('[Planes] Remove (SignalR)', props<{ id: number }>());

export const multiRemove = createAction('[Planes] Multi Remove (SignalR)', props<{ ids: number[] }>());

export const loadAllByPostSuccess = createAction(
  '[Planes] Load all by post success',
  props<{ result: DataResult<Plane[]>; event: LazyLoadEvent }>()
);

export const loadSuccess = createAction('[Planes] Load success (SignalR)', props<{ plane: Plane }>());

export const failure = createAction('[Planes] Failure (SignalR)', props<{ error: any }>());

export const openDialogEdit = createAction('[Planes] Open dialog edit (SignalR)');

export const closeDialogEdit = createAction('[Planes] Close dialog edit (SignalR)');

export const openDialogNew = createAction('[Planes] Open dialog new (SignalR)');

export const closeDialogNew = createAction('[Planes] Close dialog new (SignalR)');
