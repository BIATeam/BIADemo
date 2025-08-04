import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { BiaMessageService } from '../../services/bia-message.service';
import { TeamDas } from '../services/team-das.service';
import { BiaTeamsActions } from './teams-actions';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class TeamsEffects {
  setDefaultTeam$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BiaTeamsActions.setDefaultTeam),
      switchMap(data =>
        this.teamDas.setDefaultTeam(data.teamTypeId, data.teamId).pipe(
          switchMap(() => {
            this.biaMessageService.showUpdateSuccess();
            return [BiaTeamsActions.setDefaultTeamSuccess(data)];
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(BiaTeamsActions.failure({ error: err }));
          })
        )
      )
    )
  );
  resetDefaultTeam$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BiaTeamsActions.resetDefaultTeam),
      switchMap(data =>
        this.teamDas.resetDefaultTeam(data.teamTypeId).pipe(
          switchMap(() => {
            this.biaMessageService.showUpdateSuccess();
            return [BiaTeamsActions.resetDefaultTeamSuccess(data)];
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(BiaTeamsActions.failure({ error: err }));
          })
        )
      )
    )
  );
  setDefaultRoles$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BiaTeamsActions.setDefaultRoles),
      switchMap(data =>
        this.teamDas.setDefaultRoles(data.teamId, data.roleIds).pipe(
          switchMap(() => {
            this.biaMessageService.showUpdateSuccess();
            return [BiaTeamsActions.setDefaultRolesSuccess(data)];
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(BiaTeamsActions.failure({ error: err }));
          })
        )
      )
    )
  );
  resetDefaultRoles$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BiaTeamsActions.resetDefaultRoles),
      switchMap(data =>
        this.teamDas.resetDefaultRoles(data.teamId).pipe(
          switchMap(() => {
            this.biaMessageService.showUpdateSuccess();
            return [BiaTeamsActions.resetDefaultRolesSuccess(data)];
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(BiaTeamsActions.failure({ error: err }));
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
