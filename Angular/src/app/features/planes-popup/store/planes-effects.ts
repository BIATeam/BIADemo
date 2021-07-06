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
  remove,
  multiRemove,
  update
} from './planes-actions';
import { PlaneDas } from '../services/plane-das.service';
import { Store } from '@ngrx/store';
import { getLastLazyLoadEvent } from './plane.state';
import { Plane } from '../model/plane';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class PlanesEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.planeDas.getListByPost(event).pipe(
          map((result: DataResult<Plane[]>) => loadAllByPostSuccess({ result: result, event: event })),
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
        return this.planeDas.get(id).pipe(
          map((plane) => loadSuccess({ plane })),
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
      pluck('plane'),
      concatMap((plane) => of(plane).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([plane, event]) => {
        return this.planeDas.post(plane).pipe(
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
      pluck('plane'),
      concatMap((plane) => of(plane).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([plane, event]) => {
        return this.planeDas.put(plane, plane.id).pipe(
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
        return this.planeDas.delete(id).pipe(
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
        return this.planeDas.deletes(ids).pipe(
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
    private planeDas: PlaneDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
