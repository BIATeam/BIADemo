import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  BiaMessageService,
  BiaOnlineOfflineService,
} from 'packages/bia-ng/core/public-api';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { AnnoucementTypeOptionDas } from '../services/annoucement-type-option-das.service';
import { DomainAnnoucementTypeOptionsActions } from './annoucement-type-options-actions';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class AnnoucementTypeOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(
        DomainAnnoucementTypeOptionsActions.loadAll
      ) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the AnnoucementTypes Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'AnnoucementTypes Reducers' will take care of the rest */
      switchMap(() =>
        this.annoucementTypeOptionDas
          .getList({
            endpoint: 'allOptions',
            offlineMode: BiaOnlineOfflineService.isModeEnabled,
          })
          .pipe(
            map(annoucementTypes =>
              DomainAnnoucementTypeOptionsActions.loadAllSuccess({
                annoucementTypes: annoucementTypes?.sort((a, b) =>
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
              return of(
                DomainAnnoucementTypeOptionsActions.failure({ error: err })
              );
            })
          )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private annoucementTypeOptionDas: AnnoucementTypeOptionDas,
    private biaMessageService: BiaMessageService
  ) {}
}
