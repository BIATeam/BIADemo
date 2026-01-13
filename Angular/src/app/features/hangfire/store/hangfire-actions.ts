import { createAction, props } from '@ngrx/store';

export const failure = createAction(
  '[Notifications] Failure',
  props<{ error: any }>()
);

export const randomReviewPlane = createAction(
  '[Notifications] Call worker with notification',
  props<{ teamId: number }>()
);

export const generateUnhandledError = createAction(
  '[Notifications] Generate unhandled error'
);

export const generateHandledError = createAction(
  '[Notifications] Generate handled error'
);

export const generateErrorSuccess = createAction(
  '[Notifications] Generate error success'
);
