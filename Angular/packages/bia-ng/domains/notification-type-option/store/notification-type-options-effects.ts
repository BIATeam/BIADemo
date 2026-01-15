import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { BiaMessageService } from 'packages/bia-ng/core/public-api';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { NotificationTypeOptionDas } from '../services/notification-type-option-das.service';
import { DomainNotificationTypeOptionsActions } from './notification-type-options-actions';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class NotificationTypeOptionsEffects {
  protected actions$: Actions = inject(Actions);
  protected notificationTypeDas: NotificationTypeOptionDas = inject(
    NotificationTypeOptionDas
  );
  protected biaMessageService: BiaMessageService = inject(BiaMessageService);

  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(
        DomainNotificationTypeOptionsActions.loadAll
      ) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the NotificationTypes Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'NotificationTypes Reducers' will take care of the rest */
      switchMap(() =>
        this.notificationTypeDas.getList({ endpoint: 'allOptions' }).pipe(
          map(notificationTypes =>
            DomainNotificationTypeOptionsActions.loadAllSuccess({
              notificationTypes: notificationTypes?.sort((a, b) =>
                a.display.localeCompare(b.display)
              ),
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(
              DomainNotificationTypeOptionsActions.failure({ error: err })
            );
          })
        )
      )
    )
  );
}
