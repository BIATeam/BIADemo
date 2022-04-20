import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { DomaineUsersFromDirectoryActions } from './users-from-Directory-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { UserFromDirectoryDas } from '../services/user-from-Directory-das.service';
import { TranslateService } from '@ngx-translate/core';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class UsersFromDirectoryEffects {
  loadAllByFilter$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomaineUsersFromDirectoryActions.loadAllByFilter) /* When action is dispatched */,
      switchMap((action) => {
        return this.userFromDirectoryDas.getAllByFilter(action.userFilter.filter, action.userFilter.ldapName).pipe(
          map((users) => DomaineUsersFromDirectoryActions.loadAllSuccess({ users })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(DomaineUsersFromDirectoryActions.failure({ error: err }));
          })
        );
      })
    )
  );

  
  addFromDirectory$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomaineUsersFromDirectoryActions.addFromDirectory),
      pluck('usersFromDirectory'),
       switchMap((usersFromDirectory) => {
        return this.userFromDirectoryDas.save({ items: usersFromDirectory, endpoint: "addFromDirectory"}).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            return DomaineUsersFromDirectoryActions.addFromDirectorySuccess({ users : usersFromDirectory });
          }),
          catchError((err) => {
            if (err.status === 303) {
              let errorMessage = '';
              if (err.error) {
                err.error.forEach((element: string) => {
                  const currentError = `${this.translateService.instant('user.cannotAddMember')}`.replace(
                    '${login}',
                    element
                  );
                  if (errorMessage !== '') {
                    errorMessage += '\n';
                  }
                  errorMessage += currentError;
                });
              }
              this.biaMessageService.showErrorDetail(errorMessage);
            } else {
              this.biaMessageService.showError();
            }
            return of(DomaineUsersFromDirectoryActions.failure({ error: { concern: 'CREATE', error: err } }));
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private userFromDirectoryDas: UserFromDirectoryDas,
    private biaMessageService: BiaMessageService,
    private translateService: TranslateService,
  ) {}
}
