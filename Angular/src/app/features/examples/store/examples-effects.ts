import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { BiaMessageService } from 'packages/bia-ng/core/public-api';
import { of } from 'rxjs';
import { catchError, exhaustMap, map } from 'rxjs/operators';
import { ExamplesDas } from '../service/examples-das.service';
import {
  generateErrorFailure,
  generateErrorSuccess,
  generateFileDownloadNotification,
  generateFileDownloadNotificationFailure,
  generateFileDownloadNotificationSuccess,
  generateHandledError,
  generateUnhandledError,
} from './examples-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class ExamplesEffects {
  generateUnhandledError$ = createEffect(() =>
    this.actions$.pipe(
      ofType(generateUnhandledError),
      exhaustMap(() =>
        this.examplesDas.generateUnhandledError().pipe(
          map((): any => generateErrorSuccess()),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(generateErrorFailure());
          })
        )
      )
    )
  );

  generateHandledError$ = createEffect(() =>
    this.actions$.pipe(
      ofType(generateHandledError),
      exhaustMap(() =>
        this.examplesDas.generateHandledError().pipe(
          map((): any => generateErrorSuccess()),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(generateErrorFailure());
          })
        )
      )
    )
  );

  generateFileDownloadNotification$ = createEffect(() =>
    this.actions$.pipe(
      ofType(generateFileDownloadNotification),
      exhaustMap(() =>
        this.examplesDas.generateFileDownloadNotification().pipe(
          map((): any => generateFileDownloadNotificationSuccess()),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(generateFileDownloadNotificationFailure());
          })
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private examplesDas: ExamplesDas,
    private biaMessageService: BiaMessageService
  ) {}
}
