import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaOnlineOfflineService } from 'src/app/core/bia-core/services/bia-online-offline.service';
import { PartOptionDas } from '../services/part-option-das.service';
import { DomainPartOptionsActions } from './part-options-actions';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class PartOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainPartOptionsActions.loadAll) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the Parts Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'Parts Reducers' will take care of the rest */
      switchMap(() =>
        this.partDas
          .getList({
            endpoint: 'allOptions',
            offlineMode: BiaOnlineOfflineService.isModeEnabled,
          })
          .pipe(
            map(parts =>
              DomainPartOptionsActions.loadAllSuccess({
                parts: parts?.sort((a, b) =>
                  a.display.localeCompare(b.display)
                ),
              })
            ),
            catchError(err => {
              if (
                BiaOnlineOfflineService.isModeEnabled !== true ||
                BiaOnlineOfflineService.isServerAvailable(err) === true
              ) {
                this.biaMessageService.showErrorHttpResponse(err);
              }
              return of(DomainPartOptionsActions.failure({ error: err }));
            })
          )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private partDas: PartOptionDas,
    private biaMessageService: BiaMessageService
  ) {}
}
