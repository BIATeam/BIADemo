import { Injectable } from '@angular/core';
import {
  BiaMessageService,
  biaSuccessWaitRefreshSignalR,
} from '@bia-team/bia-ng/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { HangfireDas } from '../service/hangfire-das.service';
import {
  failure,
  prepareBackgroundDownloadFileExample,
  randomReviewPlane,
  success,
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

  prepareBackgroundDownloadFile$ = createEffect(() =>
    this.actions$.pipe(
      ofType(prepareBackgroundDownloadFileExample),
      exhaustMap(() =>
        this.hangfireDas.prepareBackgroundDownloadFileExample().pipe(
          map(() => {
            this.biaMessageService.showSuccess(
              'File is being prepared, you will be notified when it is ready for download'
            );
            return success();
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(err);
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
