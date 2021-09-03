import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap, withLatestFrom, concatMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  loadNotification,
  loadAllNotificationsByPost,
  loadAllNotificationsByPostSuccess,
  loadNotificationSuccess,
  remove,
  multiRemove,
  update,
  callWorkerWithNotification,
} from './notifications-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { Notification } from 'src/app/features/notifications/model/notification';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { getLastLazyLoadEvent } from './notification.state';
import { NotificationDas } from '../service/notification-das.service';
import { HangfireDas } from '../service/hangfire-das.service';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class NotificationsEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllNotificationsByPost),
      pluck('event'),
      switchMap((event) =>
        this.notificationDas.getListByPost(event).pipe(
          map((result: DataResult<Notification[]>) => loadAllNotificationsByPostSuccess({ result: result, event: event })),
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
      ofType(loadNotification),
      pluck('id'),
      switchMap((id) => {
        return this.notificationDas.get(id).pipe(
          map((notification) => loadNotificationSuccess({ notification })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          }));
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(update),
      pluck('notification'),
      concatMap((notification) => of(notification).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([notification, event]) => {
        return this.notificationDas.put(notification, notification.id).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return loadAllNotificationsByPost({ event: event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  callWorkerWithNotification$ = createEffect(() =>
    this.actions$.pipe(
      ofType(callWorkerWithNotification),
      concatMap((x) => of(x).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([x, event]) => {
        return this.hangfireDas.callWorkerWithNotification().pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            // return loadAllByPost({ event: event });
            return biaSuccessWaitRefreshSignalR();
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(remove),
      pluck('id'),
      concatMap((id: number) => of(id).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([id, event]) => {
        return this.notificationDas.delete(id).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return loadAllNotificationsByPost({ event: event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(multiRemove),
      pluck('ids'),
      concatMap((ids: number[]) => of(ids).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([ids, event]) => {
        return this.notificationDas.deletes(ids).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return loadAllNotificationsByPost({ event: event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private notificationDas: NotificationDas,
    private hangfireDas: HangfireDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) { }
}
