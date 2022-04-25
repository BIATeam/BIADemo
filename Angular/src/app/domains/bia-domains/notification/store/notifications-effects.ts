import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { DomainNotificationsActions } from './notifications-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { NotificationDas } from '../services/notification-das.service';

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
          map((notifications) => DomainNotificationsActions.loadAllSuccess({ notifications })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(DomainNotificationsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainNotificationsActions.load),
      pluck('id'),
      switchMap((id) =>
        this.notificationDas.get({ id: id }).pipe(
          map((notification) => DomainNotificationsActions.loadSuccess({ notification })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(DomainNotificationsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  loadUnreadNotificationIds$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainNotificationsActions.loadUnreadNotificationIds),
      pluck('event'),
      switchMap((event) =>
        this.notificationDas.getUnreadNotificationIds().pipe(
          map((ids) => DomainNotificationsActions.loadUnreadNotificationIdsSuccess({ ids })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(DomainNotificationsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  setAsRead$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainNotificationsActions.setAsRead),
      pluck('id'),
      switchMap((id) => {
        return this.notificationDas.setAsRead(id).pipe(
          map(() => DomainNotificationsActions.setAsReadSuccess()),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(DomainNotificationsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private notificationDas: NotificationDas,
    private biaMessageService: BiaMessageService
  ) { }
}
