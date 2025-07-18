import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { BiaMessageService, biaSuccessWaitRefreshSignalR } from 'biang/core';
import { DataResult } from 'biang/models';
import { BiaAppState } from 'biang/store';
import { of } from 'rxjs';
import {
  catchError,
  concatMap,
  map,
  switchMap,
  withLatestFrom,
} from 'rxjs/operators';
import { NotificationListItem } from '../model/notification-list-item';
import { notificationCRUDConfiguration } from '../notification.constants';
import { NotificationDas } from '../services/notification-das.service';
import { FeatureNotificationsStore } from './notification.state';
import { FeatureNotificationsActions } from './notifications-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class NotificationsEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureNotificationsActions.loadAllByPost),
      map(x => x?.event),
      switchMap(event =>
        this.notificationDas
          .getListItemsByPost<NotificationListItem>({ event: event })
          .pipe(
            map((result: DataResult<NotificationListItem[]>) =>
              FeatureNotificationsActions.loadAllByPostSuccess({
                result: result,
                event: event,
              })
            ),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureNotificationsActions.failure({ error: err }));
            })
          )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureNotificationsActions.load),
      map(x => x?.id),
      switchMap(id => {
        return this.notificationDas.get({ id: id }).pipe(
          map(notification =>
            FeatureNotificationsActions.loadSuccess({ notification })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  setUnread$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureNotificationsActions.setUnread),
      map(x => x?.id),
      switchMap(id => {
        return this.notificationDas.setUnread(id).pipe(
          map(notification =>
            FeatureNotificationsActions.loadSuccess({ notification })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureNotificationsActions.create),
      map(x => x?.notification),
      concatMap(notification =>
        of(notification).pipe(
          withLatestFrom(
            this.store.select(FeatureNotificationsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([notification, event]) => {
        return this.notificationDas.post({ item: notification }).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            if (notificationCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureNotificationsActions.loadAllByPost({
                event: event,
              });
            }
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureNotificationsActions.update),
      map(x => x?.notification),
      concatMap(notification =>
        of(notification).pipe(
          withLatestFrom(
            this.store.select(FeatureNotificationsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([notification, event]) => {
        return this.notificationDas
          .put({ item: notification, id: notification.id })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (notificationCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureNotificationsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureNotificationsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureNotificationsActions.remove),
      map(x => x?.id),
      concatMap((id: number) =>
        of(id).pipe(
          withLatestFrom(
            this.store.select(FeatureNotificationsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([id, event]) => {
        return this.notificationDas.delete({ id: id }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (notificationCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureNotificationsActions.loadAllByPost({
                event: event,
              });
            }
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureNotificationsActions.multiRemove),
      map(x => x?.ids),
      concatMap((ids: number[]) =>
        of(ids).pipe(
          withLatestFrom(
            this.store.select(FeatureNotificationsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([ids, event]) => {
        return this.notificationDas.deletes({ ids: ids }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (notificationCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureNotificationsActions.loadAllByPost({
                event: event,
              });
            }
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    protected actions$: Actions,
    protected notificationDas: NotificationDas,
    protected biaMessageService: BiaMessageService,
    protected store: Store<BiaAppState>
  ) {}
}
