import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  load,
  loadAllNotifications,
  loadAllSuccess,
  loadSuccess,
  loadUnreadNotificationIds,
  loadUnreadNotificationIdsSuccess,
} from './notifications-actions';
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
      ofType(loadAllNotifications),
      switchMap(() =>
        this.notificationDas.getList().pipe(
          map((notifications) => loadAllSuccess({ notifications })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(load),
      pluck('id'),
      switchMap((id) =>
        this.notificationDas.get(id).pipe(
          map((notification) => loadSuccess({ notification })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  loadUnreadNotificationIds$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadUnreadNotificationIds),
      pluck('event'),
      switchMap((event) =>
        this.notificationDas.getUnreadNotificationIds().pipe(
          map((ids) => loadUnreadNotificationIdsSuccess({ ids })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private notificationDas: NotificationDas,
    private biaMessageService: BiaMessageService
  ) { }
}
