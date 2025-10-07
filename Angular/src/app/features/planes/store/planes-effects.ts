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
import { Plane } from '../model/plane';
import { planeCRUDConfiguration } from '../plane.constants';
import { PlaneDas } from '../services/plane-das.service';
import { FeaturePlanesStore } from './plane.state';
import { FeaturePlanesActions } from './planes-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class PlanesEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.loadAllByPost),
      map(x => x?.event),
      switchMap(event =>
        this.planeDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<Plane[]>) =>
            FeaturePlanesActions.loadAllByPostSuccess({
              result: result,
              event: event,
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeaturePlanesActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.load),
      map(x => x?.id),
      switchMap(id => {
        return this.planeDas.get({ id: id }).pipe(
          map(plane => {
            this.store.dispatch(
              FeaturePlanesActions.loadHistorical({ id: plane.id })
            );
            return FeaturePlanesActions.loadSuccess({ plane });
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeaturePlanesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.create),
      map(x => x?.plane),
      concatMap(plane =>
        of(plane).pipe(
          withLatestFrom(
            this.store.select(FeaturePlanesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([plane, event]) => {
        return this.planeDas
          .post({
            item: plane,
            offlineMode: planeCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              if (planeCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeaturePlanesActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeaturePlanesActions.failure({ error: err }));
            })
          );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.update),
      map(x => x?.plane),
      concatMap(plane =>
        of(plane).pipe(
          withLatestFrom(
            this.store.select(FeaturePlanesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([plane, event]) => {
        return this.planeDas
          .put({
            item: plane,
            id: plane.id,
            offlineMode: planeCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (planeCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeaturePlanesActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeaturePlanesActions.failure({ error: err }));
            })
          );
      })
    )
  );

  save$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.save),
      map(x => x?.planes),
      concatMap(planes =>
        of(planes).pipe(
          withLatestFrom(
            this.store.select(FeaturePlanesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([planes, event]) => {
        return this.planeDas
          .save({
            items: planes,
            offlineMode: planeCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (planeCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeaturePlanesActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeaturePlanesActions.failure({ error: err }));
            })
          );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.remove),
      map(x => x?.id),
      concatMap((id: number) =>
        of(id).pipe(
          withLatestFrom(
            this.store.select(FeaturePlanesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([id, event]) => {
        return this.planeDas
          .delete({
            id: id,
            offlineMode: planeCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (planeCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeaturePlanesActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeaturePlanesActions.failure({ error: err }));
            })
          );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.multiRemove),
      map(x => x?.ids),
      concatMap((ids: number[]) =>
        of(ids).pipe(
          withLatestFrom(
            this.store.select(FeaturePlanesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([ids, event]) => {
        return this.planeDas
          .deletes({
            ids: ids,
            offlineMode: planeCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (planeCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeaturePlanesActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeaturePlanesActions.failure({ error: err }));
            })
          );
      })
    )
  );

  updateFixedStatus$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.updateFixedStatus),
      map(x => x),
      concatMap(x =>
        of(x).pipe(
          withLatestFrom(
            this.store.select(FeaturePlanesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([x, event]) => {
        return this.planeDas
          .updateFixedStatus({ id: x.id, fixed: x.isFixed })
          .pipe(
            map(plane => {
              this.biaMessageService.showUpdateSuccess();
              this.store.dispatch(
                FeaturePlanesActions.loadAllByPost({ event: event })
              );
              return FeaturePlanesActions.loadSuccess({ plane });
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeaturePlanesActions.failure({ error: err }));
            })
          );
      })
    )
  );

  loadHistorical$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.loadHistorical),
      map(x => x?.id),
      switchMap(id => {
        return this.planeDas.getHistorical({ id: id }).pipe(
          map(historical => {
            return FeaturePlanesActions.loadHistoricalSuccess({ historical });
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeaturePlanesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private planeDas: PlaneDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
