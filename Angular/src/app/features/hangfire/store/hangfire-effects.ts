import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap, withLatestFrom, concatMap, pluck } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  callWorkerWithNotification, failure,
} from './hangfire-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { getLastLazyLoadEvent } from './hangfire.state';
import { HangfireDas } from '../service/hangfire-das.service';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class HangfireEffects {
  callWorkerWithNotification$ = createEffect(() =>
    this.actions$.pipe(
      ofType(callWorkerWithNotification),
      pluck('teamId'),
      concatMap((teamId) => of(teamId).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([teamId, event]) => {
        return this.hangfireDas.callWorkerWithNotification(teamId).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            // return loadAllByPost({ event: event });
            return biaSuccessWaitRefreshSignalR();
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private hangfireDas: HangfireDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) { }
}
