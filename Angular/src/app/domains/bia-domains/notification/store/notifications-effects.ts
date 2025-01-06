import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { NotificationDas } from '../services/notification-das.service';
import { DomainNotificationsActions } from './notifications-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class NotificationsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainNotificationsActions.loadAll),
      switchMap(() =>
        this.notificationDas.getList().pipe(
          map(notifications =>
            DomainNotificationsActions.loadAllSuccess({ notifications })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(DomainNotificationsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainNotificationsActions.load),
      map(x => x?.id),
      switchMap(id =>
        this.notificationDas.get({ id: id }).pipe(
          map(notification =>
            DomainNotificationsActions.loadSuccess({ notification })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(DomainNotificationsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  loadUnreadNotificationIds$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainNotificationsActions.loadUnreadNotificationIds),
      switchMap(() =>
        this.notificationDas.getUnreadNotificationIds().pipe(
          map(ids =>
            DomainNotificationsActions.loadUnreadNotificationIdsSuccess({ ids })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(DomainNotificationsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  setAsRead$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainNotificationsActions.setAsRead),
      map(x => x?.id),
      switchMap(id => {
        return this.notificationDas.setAsRead(id).pipe(
          map(() => DomainNotificationsActions.setAsReadSuccess()),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(DomainNotificationsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    protected actions$: Actions,
    protected notificationDas: NotificationDas,
    protected biaMessageService: BiaMessageService
  ) {}
}
