import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  load,
  loadAllSites,
  loadAllSitesByUser,
  loadAllSitesByUserSuccess,
  loadAllSuccess,
  loadSuccess,
  setDefaultRoles,
  setDefaultRolesSuccess,
  setDefaultTeam,
  setDefaultTeamSuccess
} from './sites-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { SiteDas } from '../services/site-das.service';
import { MemberDas } from '../services/member-das.service';
import { LazyLoadEvent } from 'primeng/api';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class SitesEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllSites),
      switchMap(() =>
        this.siteDas.getList().pipe(
          map((sites) => loadAllSuccess({ sites })),
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
      ofType(loadAllSitesByUser),
      pluck('userId'),
      switchMap((userId) =>
        this.siteDas.getListByPost(<LazyLoadEvent>{ userId: userId }).pipe(
          map((result) => loadAllSitesByUserSuccess({ sites: result.data })),
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
        this.siteDas.get(id).pipe(
          map((site) => loadSuccess({ site })),
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
    private siteDas: SiteDas,
    private memberDas: MemberDas,
    private biaMessageService: BiaMessageService
  ) {}
}
