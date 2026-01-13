import { HttpStatusCode } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BiaMessageService } from '@bia-team/bia-ng/core';
import { DomainUserOptionsActions } from '@bia-team/bia-ng/domains';
import { OptionDto } from '@bia-team/bia-ng/models';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { TranslateService } from '@ngx-translate/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { UserFromDirectoryDas } from '../services/user-from-directory-das.service';
import { FeatureUsersFromDirectoryActions } from './users-from-directory-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class UsersFromDirectoryEffects {
  protected actions$: Actions = inject(Actions);
  protected userFromDirectoryDas: UserFromDirectoryDas =
    inject(UserFromDirectoryDas);
  protected biaMessageService: BiaMessageService = inject(BiaMessageService);
  protected translateService: TranslateService = inject(TranslateService);

  loadAllByFilter$ = createEffect(() =>
    this.actions$.pipe(
      ofType(
        FeatureUsersFromDirectoryActions.loadAllByFilter
      ) /* When action is dispatched */,
      switchMap(action => {
        return this.userFromDirectoryDas
          .getAllByFilter(
            action.userFilter.filter,
            action.userFilter.ldapName,
            action.userFilter.returnSize
          )
          .pipe(
            map(users =>
              FeatureUsersFromDirectoryActions.loadAllSuccess({ users })
            ),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(
                FeatureUsersFromDirectoryActions.failure({ error: err })
              );
            })
          );
      })
    )
  );

  addFromDirectory$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureUsersFromDirectoryActions.addFromDirectory),
      map(x => x?.usersFromDirectory),
      switchMap(usersFromDirectory => {
        return this.userFromDirectoryDas
          .save({ items: usersFromDirectory, endpoint: 'addFromDirectory' })
          .pipe(
            map((usersAdded: OptionDto[]) => {
              this.biaMessageService.showAddSuccess();
              return DomainUserOptionsActions.userAddedInListSuccess({
                usersAdded,
              });
            }),
            catchError(err => {
              if (err.status === HttpStatusCode.UnprocessableEntity) {
                let errorMessage = '';
                if (err.error) {
                  err.error.forEach((element: string) => {
                    const currentError =
                      `${this.translateService.instant('user.cannotAddMember')}`.replace(
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
                this.biaMessageService.showErrorHttpResponse(err);
              }
              return of(
                FeatureUsersFromDirectoryActions.failure({
                  error: { concern: 'CREATE', error: err },
                })
              );
            })
          );
      })
    )
  );
}
