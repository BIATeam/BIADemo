import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { BiaMessageService, biaSuccessWaitRefreshSignalR } from 'bia-ng/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { HangfireDas } from '../service/hangfire-das.service';
import { failure, randomReviewPlane } from './hangfire-actions';

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

  constructor(
    private actions$: Actions,
    private hangfireDas: HangfireDas,
    private biaMessageService: BiaMessageService
  ) {}
}
