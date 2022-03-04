import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  load,
  loadAllTeams,
  loadAllTeamsByUser,
  loadAllTeamsByUserSuccess,
  loadAllSuccess,
  loadSuccess,
  setDefaultRoles,
  setDefaultRolesSuccess,
  setDefaultTeam,
  setDefaultTeamSuccess
} from './teams-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { TeamDas } from '../services/team-das.service';
import { MemberDas } from '../services/member-das.service';
import { LazyLoadEvent } from 'primeng/api';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class TeamsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllTeams),
      switchMap(() =>
        this.teamDas.getList().pipe(
          map((teams) => loadAllSuccess({ teams })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  loadAllByUser$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllTeamsByUser),
      pluck('userId'),
      switchMap((userId) =>
        this.teamDas.getListByPost(<LazyLoadEvent>{ userId: userId }).pipe(
          map((result) => loadAllTeamsByUserSuccess({ teams: result.data })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(load),
      pluck('id'),
      switchMap((id) =>
        this.teamDas.get(id).pipe(
          map((team) => loadSuccess({ team })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  setDefaultTeam$ = createEffect(() =>
    this.actions$.pipe(
      ofType(setDefaultTeam),
      switchMap(data =>
        this.memberDas.setDefaultTeam(data.teamTypeId, data.teamId).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return setDefaultTeamSuccess();
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
          })
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private teamDas: TeamDas,
    private memberDas: MemberDas,
    private biaMessageService: BiaMessageService
  ) {}
}
