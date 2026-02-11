import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  BiaMessageService,
  BiaOnlineOfflineService,
} from 'packages/bia-ng/core/public-api';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { TeamOptionDas } from '../services/team-option-das.service';
import { DomainTeamOptionsActions } from './team-options-actions';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class TeamOptionsEffects {
  protected actions$: Actions = inject(Actions);
  protected teamDas: TeamOptionDas = inject(TeamOptionDas);
  protected biaMessageService: BiaMessageService = inject(BiaMessageService);

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
}
