import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { DomainUserOptionsActions } from './user-options-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { UserOptionDas } from '../services/user-option-das.service';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class UserOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainUserOptionsActions.loadAll) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the Users Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'Users Reducers' will take care of the rest */
      switchMap(() =>
        this.userDas.getList({ endpoint: 'allOptions' }).pipe(
          map(users =>
            DomainUserOptionsActions.loadAllSuccess({
              users: users?.sort((a, b) => a.display.localeCompare(b.display)),
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(DomainUserOptionsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  loadAllByFilter$ = createEffect(() =>
    this.actions$.pipe(
      ofType(
        DomainUserOptionsActions.loadAllByFilter
      ) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the Users Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'Users Reducers' will take care of the rest */
      switchMap(action =>
        this.userDas
          .getList({
            endpoint: 'allOptions',
            options: { params: { filter: action.filter } },
          })
          .pipe(
            map(users => DomainUserOptionsActions.loadAllSuccess({ users })),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(DomainUserOptionsActions.failure({ error: err }));
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
        this.userDas.get({ id: id }).pipe(
          map((user) => loadSuccess({ user })),
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
    private userDas: UserOptionDas,
    private biaMessageService: BiaMessageService
  ) {}
}
