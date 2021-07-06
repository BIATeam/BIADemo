import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  load,
  loadMemberRoles,
  loadAllRoles,
  loadAllSuccess,
  loadSuccess,
  loadMemberRolesSuccess,
  setDefaultRole
} from './roles-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { RoleDas } from '../services/role-das.service';
import { MemberDas } from '../../site/services/member-das.service';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class RolesEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllRoles),
      switchMap(() => {
        return this.roleDas.getList().pipe(
          map((roles) => loadAllSuccess({ roles })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      }
      )
    )
  );

  loadMemberRoles$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadMemberRoles),
      pluck('siteId'),
      switchMap((siteId) =>
        this.roleDas.getMemberRoles(siteId).pipe(
          map((roles) => loadMemberRolesSuccess({ roles })),
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
        this.roleDas.get(id).pipe(
          map((role) => loadSuccess({ role })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  setDefaultRole$ = createEffect(() =>
    this.actions$.pipe(
      ofType(setDefaultRole),
      switchMap(data =>
        this.memberDas.setDefaultRole(data.id).pipe(
          map((x) => {
            this.biaMessageService.showUpdateSuccess();
            return loadMemberRoles({ siteId: data.siteId });
          })
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private roleDas: RoleDas,
    private memberDas: MemberDas,
    private biaMessageService: BiaMessageService) { }
}
