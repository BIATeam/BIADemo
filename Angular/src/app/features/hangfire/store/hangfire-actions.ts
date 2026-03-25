import { createAction, props } from '@ngrx/store';

export const failure = createAction(
  '[Hangfire] Failure',
  props<{ error: any }>()
);

export const randomReviewPlane = createAction(
  '[Hangfire] Call worker with notification',
  props<{ teamId: number }>()
);

export const prepareBackgroundDownloadFileExample = createAction(
  '[Hangfire] Prepare background download file example'
);

export const success = createAction('[Hangfire] Success');
