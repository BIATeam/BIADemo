import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  loadAllRoleOptions,
  loadAllRoleOptionsSuccess,
} from './role-options-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { RoleOptionDas } from '../services/role-option-das.service';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class RoleOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllRoleOptions) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the Roles Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'Roles Reducers' will take care of the rest */
      switchMap(() =>
        this.roleDas.getList('allOptions').pipe(
          map((roles) => loadAllRoleOptionsSuccess({ roles })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  /*
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
  */

  constructor(
    private actions$: Actions,
    private roleDas: RoleOptionDas,
    private biaMessageService: BiaMessageService
  ) {}
}
