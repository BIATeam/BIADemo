import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { BiaMessageService } from 'packages/bia-ng/core/public-api';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { RoleOptionDas } from '../services/role-option-das.service';
import { DomainRoleOptionsActions } from './role-options-actions';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class RoleOptionsEffects {
  protected actions$: Actions = inject(Actions);
  protected roleDas: RoleOptionDas = inject(RoleOptionDas);
  protected biaMessageService: BiaMessageService = inject(BiaMessageService);

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
}
