import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import {
  BiaMessageService,
  biaSuccessWaitRefreshSignalR,
} from 'packages/bia-ng/core/public-api';
import { DataResult } from 'packages/bia-ng/models/public-api';
import { of } from 'rxjs';
import {
  catchError,
  concatMap,
  map,
  switchMap,
  withLatestFrom,
} from 'rxjs/operators';
import { AppState } from 'src/app/store/state';
import { bannerMessageCRUDConfiguration } from '../banner-message.constants';
import { BannerMessage } from '../model/banner-message';
import { BannerMessageDas } from '../services/banner-message-das.service';
import { FeatureBannerMessagesStore } from './banner-message.state';
import { FeatureBannerMessagesActions } from './banner-messages-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class BannerMessagesEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureBannerMessagesActions.loadAllByPost),
      map(x => x?.event),
      switchMap(event =>
        this.bannerMessageDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<BannerMessage[]>) =>
            FeatureBannerMessagesActions.loadAllByPostSuccess({
              result: result,
              event: event,
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureBannerMessagesActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureBannerMessagesActions.load),
      map(x => x?.id),
      switchMap(id => {
        return this.bannerMessageDas.get({ id: id }).pipe(
          map(bannerMessage => {
            if (bannerMessageCRUDConfiguration.displayHistorical) {
              this.store.dispatch(
                FeatureBannerMessagesActions.loadHistorical({
                  id: bannerMessage.id,
                })
              );
            }
            return FeatureBannerMessagesActions.loadSuccess({ bannerMessage });
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureBannerMessagesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureBannerMessagesActions.create),
      map(x => x?.bannerMessage),
      concatMap(bannerMessage =>
        of(bannerMessage).pipe(
          withLatestFrom(
            this.store.select(FeatureBannerMessagesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([bannerMessage, event]) => {
        return this.bannerMessageDas
          .post({
            item: bannerMessage,
            offlineMode: bannerMessageCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              if (bannerMessageCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureBannerMessagesActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureBannerMessagesActions.failure({ error: err }));
            })
          );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureBannerMessagesActions.update),
      map(x => x?.bannerMessage),
      concatMap(bannerMessage =>
        of(bannerMessage).pipe(
          withLatestFrom(
            this.store.select(FeatureBannerMessagesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([bannerMessage, event]) => {
        return this.bannerMessageDas
          .put({
            item: bannerMessage,
            id: bannerMessage.id,
            offlineMode: bannerMessageCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (bannerMessageCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureBannerMessagesActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureBannerMessagesActions.failure({ error: err }));
            })
          );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureBannerMessagesActions.remove),
      map(x => x?.id),
      concatMap((id: number) =>
        of(id).pipe(
          withLatestFrom(
            this.store.select(FeatureBannerMessagesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([id, event]) => {
        return this.bannerMessageDas
          .delete({
            id: id,
            offlineMode: bannerMessageCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (bannerMessageCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureBannerMessagesActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureBannerMessagesActions.failure({ error: err }));
            })
          );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureBannerMessagesActions.multiRemove),
      map(x => x?.ids),
      concatMap((ids: number[]) =>
        of(ids).pipe(
          withLatestFrom(
            this.store.select(FeatureBannerMessagesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([ids, event]) => {
        return this.bannerMessageDas
          .deletes({
            ids: ids,
            offlineMode: bannerMessageCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (bannerMessageCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureBannerMessagesActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureBannerMessagesActions.failure({ error: err }));
            })
          );
      })
    )
  );

  loadHistorical$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureBannerMessagesActions.loadHistorical),
      map(x => x?.id),
      switchMap(id => {
        return this.bannerMessageDas.getHistorical({ id: id }).pipe(
          map(historical => {
            return FeatureBannerMessagesActions.loadHistoricalSuccess({
              historical,
            });
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureBannerMessagesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  loadActives$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureBannerMessagesActions.loadActives),
      switchMap(() => {
        return this.bannerMessageDas.getActives().pipe(
          map(bannerMessages => {
            return FeatureBannerMessagesActions.loadActivesSuccess({
              actives: bannerMessages,
            });
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureBannerMessagesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private bannerMessageDas: BannerMessageDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
