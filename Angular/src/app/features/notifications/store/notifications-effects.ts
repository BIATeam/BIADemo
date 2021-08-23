import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap, withLatestFrom, concatMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  load,
  loadAllByPost,
  loadAllByPostSuccess,
  loadSuccess,
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

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class NotificationsEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.notificationDas.getListByPost(event).pipe(
          map((result: DataResult<Notification[]>) => loadAllByPostSuccess({ result: result, event: event })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ err: { concern: 'CREATE', error: err } }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(load),
      pluck('id'),
      switchMap((id) => this.notificationDas.get(id).pipe(map((notification) => loadSuccess({ notification }))))
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
            return loadAllByPost({ event: event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ err: { concern: 'CREATE', error: err } }));
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
            return loadAllByPost({ event: event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ err: { concern: 'CREATE', error: err } }));
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
            return loadAllByPost({ event: event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ err: { concern: 'CREATE', error: err } }));
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
            return loadAllByPost({ event: event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ err: { concern: 'CREATE', error: err } }));
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
