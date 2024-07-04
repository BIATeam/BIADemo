import { Inject, Injectable } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { of } from 'rxjs';
import {
  catchError,
  map,
  switchMap,
  withLatestFrom,
  concatMap,
} from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { FeatureEnginesActions } from './engines-actions';
import { Store } from '@ngrx/store';
import { FeatureEnginesStore } from './engine.state';
import { Engine } from '../model/engine';
import { engineCRUDConfiguration } from '../engine.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { EngineDas } from '../services/engine-das.service';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class EnginesEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureEnginesActions.loadAllByPost),
      map(x => x?.event),
      switchMap(event =>
        this.engineDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<Engine[]>) =>
            FeatureEnginesActions.loadAllByPostSuccess({
              result: result,
              event: event,
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureEnginesActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureEnginesActions.load),
      map(x => x?.id),
      switchMap(id => {
        if (id) {
          return this.engineDas.get({ id: id }).pipe(
            map(engine => FeatureEnginesActions.loadSuccess({ engine })),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              location.assign(this.baseHref);
              return of(FeatureEnginesActions.failure({ error: err }));
            })
          );
        } else {
          return of(FeatureEnginesActions.loadSuccess({ engine: <Engine>{} }));
        }
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureEnginesActions.create),
      map(x => x?.engine),
      concatMap(engine =>
        of(engine).pipe(
          withLatestFrom(
            this.store.select(FeatureEnginesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([engine, event]) => {
        return this.engineDas
          .post({
            item: engine,
            offlineMode: engineCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              if (engineCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureEnginesActions.loadAllByPost({
                  event: <LazyLoadEvent>event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureEnginesActions.failure({ error: err }));
            })
          );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureEnginesActions.update),
      map(x => x?.engine),
      concatMap(engine =>
        of(engine).pipe(
          withLatestFrom(
            this.store.select(FeatureEnginesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([engine, event]) => {
        return this.engineDas
          .put({
            item: engine,
            id: engine.id,
            offlineMode: engineCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (engineCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureEnginesActions.loadAllByPost({
                  event: <LazyLoadEvent>event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureEnginesActions.failure({ error: err }));
            })
          );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureEnginesActions.remove),
      map(x => x?.id),
      concatMap((id: number) =>
        of(id).pipe(
          withLatestFrom(
            this.store.select(FeatureEnginesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([id, event]) => {
        return this.engineDas
          .delete({
            id: id,
            offlineMode: engineCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (engineCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureEnginesActions.loadAllByPost({
                  event: <LazyLoadEvent>event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureEnginesActions.failure({ error: err }));
            })
          );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureEnginesActions.multiRemove),
      map(x => x?.ids),
      concatMap((ids: number[]) =>
        of(ids).pipe(
          withLatestFrom(
            this.store.select(FeatureEnginesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([ids, event]) => {
        return this.engineDas
          .deletes({
            ids: ids,
            offlineMode: engineCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (engineCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureEnginesActions.loadAllByPost({
                  event: <LazyLoadEvent>event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureEnginesActions.failure({ error: err }));
            })
          );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private engineDas: EngineDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>,
    @Inject(APP_BASE_HREF) public baseHref: string
  ) {}
}
