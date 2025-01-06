import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { TeamDas } from '../services/team-das.service';
import { DomainTeamsActions } from './teams-actions';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class TeamsEffects {
  setDefaultTeam$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainTeamsActions.setDefaultTeam),
      switchMap(data =>
        this.teamDas.setDefaultTeam(data.teamTypeId, data.teamId).pipe(
          switchMap(() => {
            this.biaMessageService.showUpdateSuccess();
            return [DomainTeamsActions.setDefaultTeamSuccess(data)];
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(DomainTeamsActions.failure({ error: err }));
          })
        )
      )
    )
  );
  setDefaultRoles$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainTeamsActions.setDefaultRoles),
      switchMap(data =>
        this.teamDas.setDefaultRoles(data.teamId, data.roleIds).pipe(
          switchMap(() => {
            this.biaMessageService.showUpdateSuccess();
            return [DomainTeamsActions.setDefaultRolesSuccess(data)];
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(DomainTeamsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  constructor(
    protected actions$: Actions,
    protected teamDas: TeamDas,
    protected biaMessageService: BiaMessageService
  ) {}
}
