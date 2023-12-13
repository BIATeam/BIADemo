import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap, withLatestFrom, concatMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { FeaturePlanesActions } from './planes-actions';
import { Store } from '@ngrx/store';
import { FeaturePlanesStore } from './plane.state';
import { Plane } from '../model/plane';
import { PlaneCRUDConfiguration } from '../plane.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { PlaneDas } from '../services/plane-das.service';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class PlanesEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.planeDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<Plane[]>) => FeaturePlanesActions.loadAllByPostSuccess({ result: result, event: event })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeaturePlanesActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.load),
      pluck('id'),
      switchMap((id) => {
        if (id) {
          return this.planeDas.get({ id: id }).pipe(
            map((plane) => FeaturePlanesActions.loadSuccess({ plane })),
            catchError((err) => {
              this.biaMessageService.showError();
              return of(FeaturePlanesActions.failure({ error: err }));
            })
          );
        } else {
          return of(FeaturePlanesActions.loadSuccess({ plane: <Plane>{} }));
        }
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.create),
      pluck('plane'),
      concatMap((plane) => of(plane).pipe(withLatestFrom(this.store.select(FeaturePlanesStore.getLastLazyLoadEvent)))),
      switchMap(([plane, event]) => {
        return this.planeDas.post({ item: plane, offlineMode: PlaneCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            if (PlaneCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeaturePlanesActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeaturePlanesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.update),
      pluck('plane'),
      concatMap((plane) => of(plane).pipe(withLatestFrom(this.store.select(FeaturePlanesStore.getLastLazyLoadEvent)))),
      switchMap(([plane, event]) => {
        return this.planeDas.put({ item: plane, id: plane.id, offlineMode: PlaneCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            if (PlaneCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeaturePlanesActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeaturePlanesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.remove),
      pluck('id'),
      concatMap((id: number) => of(id).pipe(withLatestFrom(this.store.select(FeaturePlanesStore.getLastLazyLoadEvent)))),
      switchMap(([id, event]) => {
        return this.planeDas.delete({ id: id, offlineMode: PlaneCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (PlaneCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeaturePlanesActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeaturePlanesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesActions.multiRemove),
      pluck('ids'),
      concatMap((ids: number[]) => of(ids).pipe(withLatestFrom(this.store.select(FeaturePlanesStore.getLastLazyLoadEvent)))),
      switchMap(([ids, event]) => {
        return this.planeDas.deletes({ ids: ids, offlineMode: PlaneCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (PlaneCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeaturePlanesActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
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
  ) { }
}
