import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import {
  catchError,
  map,
  switchMap,
  withLatestFrom,
  concatMap,
} from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { FeatureUsersActions } from './users-actions';
import { Store } from '@ngrx/store';
import { FeatureUsersStore } from './user.state';
import { User } from '../model/user';
import { userCRUDConfiguration } from '../user.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { UserDas } from '../services/user-das.service';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class UsersEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureUsersActions.loadAllByPost),
      map(x => x?.event),
      switchMap(event =>
        this.userDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<User[]>) =>
            FeatureUsersActions.loadAllByPostSuccess({
              result: result,
              event: event,
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureUsersActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureUsersActions.load),
      map(x => x?.id),
      switchMap(id => {
        return this.userDas.get({ id: id }).pipe(
          map(user => FeatureUsersActions.loadSuccess({ user })),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureUsersActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureUsersActions.create),
      map(x => x?.user),
      concatMap(user =>
        of(user).pipe(
          withLatestFrom(
            this.store.select(FeatureUsersStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([user, event]) => {
        return this.userDas
          .post({
            item: user,
            offlineMode: userCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              if (userCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureUsersActions.loadAllByPost({
                  event: <LazyLoadEvent>event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureUsersActions.failure({ error: err }));
            })
          );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureUsersActions.update),
      map(x => x?.user),
      concatMap(user =>
        of(user).pipe(
          withLatestFrom(
            this.store.select(FeatureUsersStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([user, event]) => {
        return this.userDas
          .put({
            item: user,
            id: user.id,
            offlineMode: userCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (userCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureUsersActions.loadAllByPost({
                  event: <LazyLoadEvent>event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureUsersActions.failure({ error: err }));
            })
          );
      })
    )
  );

  save$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureUsersActions.save),
      map(x => x?.users),
      concatMap(users =>
        of(users).pipe(
          withLatestFrom(
            this.store.select(FeatureUsersStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([users, event]) => {
        return this.userDas
          .save({
            items: users,
            offlineMode: userCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (userCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureUsersActions.loadAllByPost({
                  event: <LazyLoadEvent>event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureUsersActions.failure({ error: err }));
            })
          );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureUsersActions.remove),
      map(x => x?.id),
      concatMap((id: number) =>
        of(id).pipe(
          withLatestFrom(
            this.store.select(FeatureUsersStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([id, event]) => {
        return this.userDas
          .delete({ id: id, offlineMode: userCRUDConfiguration.useOfflineMode })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (userCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureUsersActions.loadAllByPost({
                  event: <LazyLoadEvent>event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureUsersActions.failure({ error: err }));
            })
          );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureUsersActions.multiRemove),
      map(x => x?.ids),
      concatMap((ids: number[]) =>
        of(ids).pipe(
          withLatestFrom(
            this.store.select(FeatureUsersStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([ids, event]) => {
        return this.userDas
          .deletes({
            ids: ids,
            offlineMode: userCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (userCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureUsersActions.loadAllByPost({
                  event: <LazyLoadEvent>event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureUsersActions.failure({ error: err }));
            })
          );
      })
    )
  );

  synchronize$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureUsersActions.synchronize),
      withLatestFrom(this.store.select(FeatureUsersStore.getLastLazyLoadEvent)),
      switchMap(([, event]) => {
        return this.userDas.synchronize().pipe(
          map(() => {
            this.biaMessageService.showSyncSuccess();
            return FeatureUsersActions.loadAllByPost({ event: event });
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(
              FeatureUsersActions.failure({
                error: { concern: 'CREATE', error: err },
              })
            );
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private userDas: UserDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
