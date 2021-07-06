import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { failure, loadAllSuccess, loadAllByFilter } from './users-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { UserDas } from '../services/user-das.service';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class UsersEffects {

  loadAllByFilter$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllByFilter) /* When action is dispatched */,
      switchMap((action) => {
        return this.userDas.getAllByFilter(action.filter).pipe(
          map((users) => loadAllSuccess({ users })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(private actions$: Actions, private userDas: UserDas, private biaMessageService: BiaMessageService) {}
}
