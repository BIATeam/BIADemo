import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { DomainTeamOptionsActions } from './team-options-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { TeamOptionDas } from '../services/team-option-das.service';
import { BiaOnlineOfflineService } from 'src/app/core/bia-core/services/bia-online-offline.service';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class TeamOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainTeamOptionsActions.loadAll) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the Teams Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'Teams Reducers' will take care of the rest */
      switchMap(() =>
        this.teamDas
          .getList({
            endpoint: 'allOptions',
            offlineMode: BiaOnlineOfflineService.isModeEnabled,
          })
          .pipe(
            map(teams =>
              DomainTeamOptionsActions.loadAllSuccess({
                teams: teams?.sort((a, b) =>
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
              return of(DomainTeamOptionsActions.failure({ error: err }));
            })
          )
      )
    )
  );

  constructor(
    protected actions$: Actions,
    protected teamDas: TeamOptionDas,
    protected biaMessageService: BiaMessageService
  ) {}
}
