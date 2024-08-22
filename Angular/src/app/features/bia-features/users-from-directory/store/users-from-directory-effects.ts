import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { FeatureUsersFromDirectoryActions } from './users-from-directory-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { UserFromDirectoryDas } from '../services/user-from-directory-das.service';
import { TranslateService } from '@ngx-translate/core';
import { DomainUserOptionsActions } from 'src/app/domains/bia-domains/user-option/store/user-options-actions';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { HttpStatusCode } from '@angular/common/http';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class UsersFromDirectoryEffects {
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

  constructor(
    protected actions$: Actions,
    protected userFromDirectoryDas: UserFromDirectoryDas,
    protected biaMessageService: BiaMessageService,
    protected translateService: TranslateService
  ) {}
}
