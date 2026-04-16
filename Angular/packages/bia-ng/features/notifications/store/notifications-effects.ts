import { inject, Injectable, InjectionToken } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import {
  BiaMessageService,
  biaSuccessWaitRefreshSignalR,
} from 'packages/bia-ng/core/public-api';
import { DataResult } from 'packages/bia-ng/models/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
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
 * Injection token for the notification CRUD configuration used by effects.
 * Override this in BiaNotificationModule.forFeature() providers to use a custom config.
 */
export const NOTIFICATION_CRUD_CONFIG = new InjectionToken(
  'NOTIFICATION_CRUD_CONFIG',
  { providedIn: 'root', factory: () => notificationCRUDConfiguration }
);

@Injectable()
export class NotificationsEffects {
  protected actions$: Actions = inject(Actions);
  protected notificationDas: NotificationDas = inject(NotificationDas);
  protected biaMessageService: BiaMessageService = inject(BiaMessageService);
  protected store: Store<BiaAppState> = inject(Store<BiaAppState>);
  protected crudConfig = inject(NOTIFICATION_CRUD_CONFIG);

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
      switchMap(id =>
        this.notificationDas.get({ id: id }).pipe(
          map(notification =>
            FeatureNotificationsActions.loadSuccess({ notification })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  setUnread$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureNotificationsActions.setUnread),
      map(x => x?.id),
      switchMap(id =>
        this.notificationDas.setUnread(id).pipe(
          map(notification =>
            FeatureNotificationsActions.loadSuccess({ notification })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        )
      )
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
      switchMap(([notification, event]) =>
        this.notificationDas.post({ item: notification }).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            return this.crudConfig.useSignalR
              ? biaSuccessWaitRefreshSignalR()
              : FeatureNotificationsActions.loadAllByPost({ event });
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        )
      )
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
      switchMap(([notification, event]) =>
        this.notificationDas
          .put({ item: notification, id: notification.id })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              return this.crudConfig.useSignalR
                ? biaSuccessWaitRefreshSignalR()
                : FeatureNotificationsActions.loadAllByPost({ event });
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureNotificationsActions.failure({ error: err }));
            })
          )
      )
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
      switchMap(([id, event]) =>
        this.notificationDas.delete({ id: id }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return this.crudConfig.useSignalR
              ? biaSuccessWaitRefreshSignalR()
              : FeatureNotificationsActions.loadAllByPost({ event });
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        )
      )
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
      switchMap(([ids, event]) =>
        this.notificationDas.deletes({ ids: ids }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return this.crudConfig.useSignalR
              ? biaSuccessWaitRefreshSignalR()
              : FeatureNotificationsActions.loadAllByPost({ event });
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureNotificationsActions.failure({ error: err }));
          })
        )
      )
    )
  );
}
