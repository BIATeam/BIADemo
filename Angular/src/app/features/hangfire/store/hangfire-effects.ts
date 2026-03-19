import { Injectable } from '@angular/core';
import {
  BiaMessageService,
  biaSuccessWaitRefreshSignalR,
} from '@bia-team/bia-ng/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Action } from '@ngrx/store';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { HangfireDas } from '../service/hangfire-das.service';
import {
  failure,
  generateErrorSuccess,
  generateHandledError,
  generateUnhandledError,
  randomReviewPlane,
} from './hangfire-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class HangfireEffects {
  randomReviewPlane$ = createEffect(() =>
    this.actions$.pipe(
      ofType(randomReviewPlane),
      map(x => x?.teamId),
      switchMap(teamId => {
        return this.hangfireDas.randomReviewPlane(teamId).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return biaSuccessWaitRefreshSignalR();
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  generateUnhandledError$ = createEffect(() =>
    this.actions$.pipe(
      ofType(generateUnhandledError),
      exhaustMap(() =>
        this.hangfireDas.generateUnhandledError().pipe(
          map((): Action => generateErrorSuccess()),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  generateHandledError$ = createEffect(() =>
    this.actions$.pipe(
      ofType(generateHandledError),
      exhaustMap(() =>
        this.hangfireDas.generateHandledError().pipe(
          map((): Action => generateErrorSuccess()),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private hangfireDas: HangfireDas,
    private biaMessageService: BiaMessageService
  ) {}
}
