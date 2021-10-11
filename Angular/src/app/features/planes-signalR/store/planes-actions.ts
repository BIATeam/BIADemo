import { createAction, props } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Plane } from '../model/plane';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export const loadAllByPost = createAction('[Planes SignalR] Load all by post', props<{ event: LazyLoadEvent }>());

export const load = createAction('[Planes SignalR] Load', props<{ id: number }>());

export const create = createAction('[Planes SignalR] Create', props<{ plane: Plane }>());

export const update = createAction('[Planes SignalR] Update', props<{ plane: Plane }>());

export const remove = createAction('[Planes SignalR] Remove', props<{ id: number }>());

export const multiRemove = createAction('[Planes SignalR] Multi Remove', props<{ ids: number[] }>());

export const loadAllByPostSuccess = createAction(
  '[Planes SignalR] Load all by post success',
  props<{ result: DataResult<Plane[]>; event: LazyLoadEvent }>()
);

export const loadSuccess = createAction('[Planes SignalR] Load success', props<{ plane: Plane }>());

export const failure = createAction('[Planes SignalR] Failure', props<{ error: any }>());

export const openDialogEdit = createAction('[Planes SignalR] Open dialog edit');

export const closeDialogEdit = createAction('[Planes SignalR] Close dialog edit');

export const openDialogNew = createAction('[Planes SignalR] Open dialog new');

export const closeDialogNew = createAction('[Planes SignalR] Close dialog new');
