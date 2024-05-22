import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap, withLatestFrom, concatMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { FeaturePlanesTypesActions } from './planes-types-actions';
import { Store } from '@ngrx/store';
import { FeaturePlanesTypesStore } from './plane-type.state';
import { PlaneType } from '../model/plane-type';
import { PlaneTypeCRUDConfiguration } from '../plane-type.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { PlaneTypeDas } from '../services/plane-type-das.service';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class PlanesTypesEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesTypesActions.loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.planeTypeDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<PlaneType[]>) => FeaturePlanesTypesActions.loadAllByPostSuccess({ result: result, event: event })),
          catchError((err) => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeaturePlanesTypesActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesTypesActions.load),
      pluck('id'),
      switchMap((id) => {
        if (id) {
          return this.planeTypeDas.get({ id: id }).pipe(
            map((planeType) => FeaturePlanesTypesActions.loadSuccess({ planeType })),
            catchError((err) => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeaturePlanesTypesActions.failure({ error: err }));
            })
          );
        } else {
          return of(FeaturePlanesTypesActions.loadSuccess({ planeType: <PlaneType>{} }));
        }
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesTypesActions.create),
      pluck('planeType'),
      concatMap((planeType) => of(planeType).pipe(withLatestFrom(this.store.select(FeaturePlanesTypesStore.getLastLazyLoadEvent)))),
      switchMap(([planeType, event]) => {
        return this.planeTypeDas.post({ item: planeType, offlineMode: PlaneTypeCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            if (PlaneTypeCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeaturePlanesTypesActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeaturePlanesTypesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesTypesActions.update),
      pluck('planeType'),
      concatMap((planeType) => of(planeType).pipe(withLatestFrom(this.store.select(FeaturePlanesTypesStore.getLastLazyLoadEvent)))),
      switchMap(([planeType, event]) => {
        return this.planeTypeDas.put({ item: planeType, id: planeType.id, offlineMode: PlaneTypeCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            if (PlaneTypeCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeaturePlanesTypesActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeaturePlanesTypesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesTypesActions.remove),
      pluck('id'),
      concatMap((id: number) => of(id).pipe(withLatestFrom(this.store.select(FeaturePlanesTypesStore.getLastLazyLoadEvent)))),
      switchMap(([id, event]) => {
        return this.planeTypeDas.delete({ id: id, offlineMode: PlaneTypeCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (PlaneTypeCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeaturePlanesTypesActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeaturePlanesTypesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesTypesActions.multiRemove),
      pluck('ids'),
      concatMap((ids: number[]) => of(ids).pipe(withLatestFrom(this.store.select(FeaturePlanesTypesStore.getLastLazyLoadEvent)))),
      switchMap(([ids, event]) => {
        return this.planeTypeDas.deletes({ ids: ids, offlineMode: PlaneTypeCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (PlaneTypeCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeaturePlanesTypesActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeaturePlanesTypesActions.failure({ error: err }));
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
  ) { }
}
