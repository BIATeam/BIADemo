import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { BiaMessageService } from '../../services/bia-message.service';
import { NotificationDas } from '../services/notification-das.service';
import { CoreNotificationsActions } from './notifications-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class NotificationsEffects {
  protected actions$: Actions = inject(Actions);
  protected notificationDas: NotificationDas = inject(NotificationDas);
  protected biaMessageService: BiaMessageService = inject(BiaMessageService);

  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CoreNotificationsActions.loadAll),
      switchMap(() =>
        this.notificationDas.getList().pipe(
          map(notifications =>
            CoreNotificationsActions.loadAllSuccess({ notifications })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(CoreNotificationsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CoreNotificationsActions.load),
      map(x => x?.id),
      switchMap(id =>
        this.notificationDas.get({ id: id }).pipe(
          map(notification =>
            CoreNotificationsActions.loadSuccess({ notification })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(CoreNotificationsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  loadUnreadNotificationIds$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CoreNotificationsActions.loadUnreadNotificationIds),
      switchMap(() =>
        this.notificationDas.getUnreadNotificationIds().pipe(
          map(ids =>
            CoreNotificationsActions.loadUnreadNotificationIdsSuccess({ ids })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(CoreNotificationsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  setAsRead$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CoreNotificationsActions.setAsRead),
      map(x => x?.id),
      switchMap(id => {
        return this.notificationDas.setAsRead(id).pipe(
          map(() => CoreNotificationsActions.setAsReadSuccess()),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(CoreNotificationsActions.failure({ error: err }));
          })
        );
      })
    )
  );
}
