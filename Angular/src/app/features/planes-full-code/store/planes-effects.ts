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
import { FeaturePlanesActions } from './planes-actions';
import { PlaneDas } from '../services/plane-das.service';
import { Store } from '@ngrx/store';
import { getLastLazyLoadEvent } from './plane.state';
import { Plane } from '../model/plane';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { useSignalR } from '../plane.constants';

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
          map(plane => FeaturePlanesActions.loadSuccess({ plane })),
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
        of(plane).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))
      ),
      switchMap(([plane, event]) => {
        return this.planeDas.post({ item: plane }).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            if (useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeaturePlanesActions.loadAllByPost({
                event: <LazyLoadEvent>event,
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
        of(plane).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))
      ),
      switchMap(([plane, event]) => {
        return this.planeDas.put({ item: plane, id: plane.id }).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            if (useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeaturePlanesActions.loadAllByPost({
                event: <LazyLoadEvent>event,
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
        of(id).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))
      ),
      switchMap(([id, event]) => {
        return this.planeDas.delete({ id: id }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeaturePlanesActions.loadAllByPost({
                event: <LazyLoadEvent>event,
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
        of(ids).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))
      ),
      switchMap(([ids, event]) => {
        return this.planeDas.deletes({ ids: ids }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeaturePlanesActions.loadAllByPost({
                event: <LazyLoadEvent>event,
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

  constructor(
    private actions$: Actions,
    private planeDas: PlaneDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
