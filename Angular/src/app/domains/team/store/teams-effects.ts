import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure, setDefaultRoles, setDefaultRolesSuccess, setDefaultTeam, setDefaultTeamSuccess,
} from './teams-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { MemberDas } from '../services/member-das.service';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class TeamsEffects {

  setDefaultTeam$ = createEffect(() =>
    this.actions$.pipe(
      ofType(setDefaultTeam),
      switchMap(data =>
        this.memberDas.setDefaultTeam(data.teamTypeId, data.teamId).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return setDefaultTeamSuccess();
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );
  setDefaultRoles$ = createEffect(() =>
    this.actions$.pipe(
      ofType(setDefaultRoles),
      switchMap(data =>
        this.memberDas.setDefaultRoles(data.teamId, data.roleIds).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return setDefaultRolesSuccess();
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private memberDas: MemberDas,
    private biaMessageService: BiaMessageService
  ) {}
}
