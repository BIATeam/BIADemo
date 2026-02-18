import { createAction, props } from '@ngrx/store';

export const failure = createAction(
  '[Notifications] Failure',
  props<{ error: any }>()
);

export const randomReviewPlane = createAction(
  '[Notifications] Call worker with notification',
  props<{ teamId: number }>()
);
