import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { randomReviewPlane, failure } from './hangfire-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { HangfireDas } from '../service/hangfire-das.service';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';

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
