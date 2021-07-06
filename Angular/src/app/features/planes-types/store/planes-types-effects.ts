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
  multiRemove,
  remove,
  update
} from './planes-types-actions';
import { PlaneTypeDas } from '../services/plane-type-das.service';
import { Store } from '@ngrx/store';
import { getLastLazyLoadEvent } from './plane-type.state';
import { PlaneType } from '../model/plane-type';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class PlanesTypesEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.planeTypeDas.getListByPost(event).pipe(
          map((result: DataResult<PlaneType[]>) => loadAllByPostSuccess({ result: result, event: event })),
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
        return this.planeTypeDas.get(id).pipe(
          map((planeType) => loadSuccess({ planeType })),
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
      pluck('planeType'),
      concatMap((planeType) => of(planeType).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([planeType, event]) => {
        return this.planeTypeDas.post(planeType).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            // Uncomment this if you do not use SignalR to refresh
            return loadAllByPost({ event: <LazyLoadEvent>event });
            // Uncomment this if you use SignalR to refresh
            // return biaSuccessWaitRefreshSignalR();
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
      pluck('planeType'),
      concatMap((planeType) => of(planeType).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([planeType, event]) => {
        return this.planeTypeDas.put(planeType, planeType.id).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            // Uncomment this if you do not use SignalR to refresh
            return loadAllByPost({ event: <LazyLoadEvent>event });
            // Uncomment this if you use SignalR to refresh
            // return biaSuccessWaitRefreshSignalR();
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
        return this.planeTypeDas.delete(id).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            // Uncomment this if you do not use SignalR to refresh
            return loadAllByPost({ event: <LazyLoadEvent>event });
            // Uncomment this if you use SignalR to refresh
            // return biaSuccessWaitRefreshSignalR();
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
        return this.planeTypeDas.deletes(ids).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            // Uncomment this if you do not use SignalR to refresh
            return loadAllByPost({ event: <LazyLoadEvent>event });
            // Uncomment this if you use SignalR to refresh
            // return biaSuccessWaitRefreshSignalR();
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
    private planeTypeDas: PlaneTypeDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
