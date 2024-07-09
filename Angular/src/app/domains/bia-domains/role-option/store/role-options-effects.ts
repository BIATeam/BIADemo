import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { RoleOptionDas } from '../services/role-option-das.service';
import { DomainRoleOptionsActions } from './role-options-actions';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class RoleOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainRoleOptionsActions.loadAll) /* When action is dispatched */,
      map(x => x?.teamTypeId),
      /* startWith(loadAll()), */
      /* Hit the Roles Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'Roles Reducers' will take care of the rest */
      switchMap(teamTypeId =>
        this.roleDas
          .getList({ endpoint: 'allOptions?teamTypeId=' + teamTypeId })
          .pipe(
            map(roles =>
              DomainRoleOptionsActions.loadAllSuccess({
                roles: roles?.sort((a, b) =>
                  a.display.localeCompare(b.display)
                ),
              })
            ),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(DomainRoleOptionsActions.failure({ error: err }));
            })
          )
      )
    )
  );

  /*
  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(load),
      map(x => x?.id),
      switchMap((id) =>
        this.roleDas.get({ id: id }).pipe(
          map((role) => loadSuccess({ role })),
          catchError((err) => {
            this.biaMessageService.showErrorHttpResponse(err);
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
