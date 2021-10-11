import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap, withLatestFrom, concatMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  create,
  failure,
  load,
  loadAllByPost,
  loadAllByPostSuccess,
  loadSuccess,
  remove,
  multiRemove,
  update
} from './notifications-actions';
import { NotificationDas } from '../services/notification-das.service';
import { Store } from '@ngrx/store';
import { getLastLazyLoadEvent } from './notification.state';
import { Notification } from '../model/notification';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class NotificationsEffects {
  static useSignalR = false;
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.notificationDas.getListByPost(event).pipe(
          map((result: DataResult<Notification[]>) => loadAllByPostSuccess({ result: result, event: event })),
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
      switchMap((id) => {
        return this.notificationDas.get(id).pipe(
          map((notification) => loadSuccess({ notification })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(create),
      pluck('notification'),
      concatMap((notification) => of(notification).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([notification, event]) => {
        return this.notificationDas.post(notification).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            if (NotificationsEffects.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
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
            if (NotificationsEffects.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return loadAllByPost({ event: <LazyLoadEvent>event });
            }
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
            if (NotificationsEffects.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return loadAllByPost({ event: <LazyLoadEvent>event });
            }
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
            if (NotificationsEffects.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return loadAllByPost({ event: <LazyLoadEvent>event });
            }
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
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
