import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { failure, loadAllSuccess, loadAllByFilter } from './users-from-AD-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { UserFromADDas } from '../services/user-from-AD-das.service';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class UsersFromADEffects {
  loadAllByFilter$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllByFilter) /* When action is dispatched */,
      switchMap((action) => {
        return this.userFromADDas.getAllByFilter(action.userFilter.filter, action.userFilter.ldapName).pipe(
          map((users) => loadAllSuccess({ users })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private userFromADDas: UserFromADDas,
    private biaMessageService: BiaMessageService
  ) {}
}
