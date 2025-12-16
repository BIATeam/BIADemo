import { Injectable } from '@angular/core';
import {
  BiaMessageService,
  biaSuccessWaitRefreshSignalR,
} from '@bia-team/bia-ng/core';
import { DataResult } from '@bia-team/bia-ng/models';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import {
  catchError,
  concatMap,
  map,
  switchMap,
  withLatestFrom,
} from 'rxjs/operators';
import { AppState } from 'src/app/store/state';
import { PlaneType } from '../model/plane-type';
import { planeTypeCRUDConfiguration } from '../plane-type.constants';
import { PlaneTypeDas } from '../services/plane-type-das.service';
import { FeaturePlanesTypesStore } from './plane-type.state';
import { FeaturePlanesTypesActions } from './planes-types-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class PlanesTypesEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesTypesActions.loadAllByPost),
      map(x => x?.event),
      switchMap(event =>
        this.planeTypeDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<PlaneType[]>) =>
            FeaturePlanesTypesActions.loadAllByPostSuccess({
              result: result,
              event: event,
            })
          ),
          catchError(err => {
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
      map(x => x?.id),
      switchMap(id => {
        return this.planeTypeDas.get({ id: id }).pipe(
          map(planeType =>
            FeaturePlanesTypesActions.loadSuccess({ planeType })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeaturePlanesTypesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePlanesTypesActions.create),
      map(x => x?.planeType),
      concatMap(planeType =>
        of(planeType).pipe(
          withLatestFrom(
            this.store.select(FeaturePlanesTypesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([planeType, event]) => {
        return this.planeTypeDas
          .post({
            item: planeType,
            offlineMode: planeTypeCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              if (planeTypeCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeaturePlanesTypesActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
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
      map(x => x?.planeType),
      concatMap(planeType =>
        of(planeType).pipe(
          withLatestFrom(
            this.store.select(FeaturePlanesTypesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([planeType, event]) => {
        return this.planeTypeDas
          .put({
            item: planeType,
            id: planeType.id,
            offlineMode: planeTypeCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (planeTypeCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeaturePlanesTypesActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
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
      map(x => x?.id),
      concatMap((id: number) =>
        of(id).pipe(
          withLatestFrom(
            this.store.select(FeaturePlanesTypesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([id, event]) => {
        return this.planeTypeDas
          .delete({
            id: id,
            offlineMode: planeTypeCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (planeTypeCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeaturePlanesTypesActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
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
      map(x => x?.ids),
      concatMap((ids: number[]) =>
        of(ids).pipe(
          withLatestFrom(
            this.store.select(FeaturePlanesTypesStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([ids, event]) => {
        return this.planeTypeDas
          .deletes({
            ids: ids,
            offlineMode: planeTypeCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (planeTypeCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeaturePlanesTypesActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
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
  ) {}
}
