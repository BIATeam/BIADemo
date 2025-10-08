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
import { engineCRUDConfiguration } from '../engine.constants';
import { Engine } from '../model/engine';
import { EngineDas } from '../services/engine-das.service';
import { FeatureEnginesStore } from './engine.state';
import { FeatureEnginesActions } from './engines-actions';

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
        return this.engineDas.get({ id: id }).pipe(
          map(engine => FeatureEnginesActions.loadSuccess({ engine })),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureEnginesActions.failure({ error: err }));
          })
        );
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
                  event: event,
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
                  event: event,
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

  save$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureEnginesActions.save),
      map(x => x?.engines),
      concatMap(engines =>
        of(engines).pipe(
          withLatestFrom(
            this.store.select(FeatureEnginesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([engines, event]) => {
        return this.engineDas
          .save({
            items: engines,
            offlineMode: engineCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (engineCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureEnginesActions.loadAllByPost({
                  event: event,
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
                  event: event,
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
                  event: event,
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

  updateFixedStatus$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureEnginesActions.updateFixedStatus),
      map(x => x),
      concatMap(x =>
        of(x).pipe(
          withLatestFrom(
            this.store.select(FeatureEnginesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([x, event]) => {
        return this.engineDas
          .updateFixedStatus({ id: x.id, fixed: x.isFixed })
          .pipe(
            map(_ => {
              this.biaMessageService.showUpdateSuccess();
              this.store.dispatch(FeatureEnginesActions.load({ id: x.id }));
              return FeatureEnginesActions.loadAllByPost({ event: event });
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
    private store: Store<AppState>
  ) {}
}
