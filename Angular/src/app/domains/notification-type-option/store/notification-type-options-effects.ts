import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  loadAllNotificationTypeOptions,
  loadAllSuccess
} from './notification-type-options-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { NotificationTypeOptionDas } from '../services/notification-type-option-das.service';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class NotificationTypeOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllNotificationTypeOptions) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the NotificationTypes Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'NotificationTypes Reducers' will take care of the rest */
      switchMap(() =>
        this.notificationTypeDas.getList('allOptions').pipe(
          map((notificationTypes) => loadAllSuccess({ notificationTypes })),
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
        this.notificationTypeDas.get(id).pipe(
          map((notificationType) => loadSuccess({ notificationType })),
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
    private notificationTypeDas: NotificationTypeOptionDas,
    private biaMessageService: BiaMessageService
  ) {}
}
