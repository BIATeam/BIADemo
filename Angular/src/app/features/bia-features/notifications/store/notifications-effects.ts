import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap, withLatestFrom, concatMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { FeatureNotificationsActions } from './notifications-actions';
import { NotificationDas } from '../services/notification-das.service';
import { Store } from '@ngrx/store';
import { getLastLazyLoadEvent } from './notification.state';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { NotificationListItem } from '../model/notificationListItem';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class NotificationsEffects {
  static useSignalR = false;
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureNotificationsActions.loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.notificationDas.getListItemsByPost<NotificationListItem>({ event: event }).pipe(
          map((result: DataResult<NotificationListItem[]>) => FeatureNotificationsActions.loadAllByPostSuccess({ result: result, event: event })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureNotificationsActions.load),
      pluck('id'),
      switchMap((id) => {
        return this.notificationDas.get({ id: id }).pipe(
          map((notification) => FeatureNotificationsActions.loadSuccess({ notification })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureNotificationsActions.create),
      pluck('notification'),
      concatMap((notification) => of(notification).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([notification, event]) => {
        return this.notificationDas.post({ item: notification }).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            if (NotificationsEffects.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureNotificationsActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureNotificationsActions.update),
      pluck('notification'),
      concatMap((notification) => of(notification).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([notification, event]) => {
        return this.notificationDas.put({ item: notification, id: notification.id }).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            if (NotificationsEffects.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureNotificationsActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureNotificationsActions.remove),
      pluck('id'),
      concatMap((id: number) => of(id).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([id, event]) => {
        return this.notificationDas.delete({ id: id }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (NotificationsEffects.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureNotificationsActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureNotificationsActions.multiRemove),
      pluck('ids'),
      concatMap((ids: number[]) => of(ids).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([ids, event]) => {
        return this.notificationDas.deletes({ ids: ids }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (NotificationsEffects.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureNotificationsActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureNotificationsActions.failure({ error: err }));
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
