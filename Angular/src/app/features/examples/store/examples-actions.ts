import { createAction } from '@ngrx/store';

export const generateUnhandledError = createAction(
  '[Examples] Generate unhandled error'
);

export const generateHandledError = createAction(
  '[Examples] Generate handled error'
);

export const generateErrorSuccess = createAction(
  '[Examples] Generate error success'
);
