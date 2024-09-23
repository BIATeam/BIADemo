import { Injectable } from '@angular/core';
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
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { airportCRUDConfiguration } from '../airport.constants';
import { Airport } from '../model/airport';
import { AirportDas } from '../services/airport-das.service';
import { FeatureAirportsStore } from './airport.state';
import { FeatureAirportsActions } from './airports-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class AirportsEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAirportsActions.loadAllByPost),
      map(x => x?.event),
      switchMap(event =>
        this.airportDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<Airport[]>) =>
            FeatureAirportsActions.loadAllByPostSuccess({
              result: result,
              event: event,
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureAirportsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAirportsActions.load),
      map(x => x?.id),
      switchMap(id => {
        return this.airportDas.get({ id: id }).pipe(
          map(airport => FeatureAirportsActions.loadSuccess({ airport })),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureAirportsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAirportsActions.create),
      map(x => x?.airport),
      concatMap(airport =>
        of(airport).pipe(
          withLatestFrom(
            this.store.select(FeatureAirportsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([airport, event]) => {
        return this.airportDas
          .post({
            item: airport,
            offlineMode: airportCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              if (airportCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAirportsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureAirportsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAirportsActions.update),
      map(x => x?.airport),
      concatMap(airport =>
        of(airport).pipe(
          withLatestFrom(
            this.store.select(FeatureAirportsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([airport, event]) => {
        return this.airportDas
          .put({
            item: airport,
            id: airport.id,
            offlineMode: airportCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (airportCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAirportsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureAirportsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAirportsActions.remove),
      map(x => x?.id),
      concatMap((id: number) =>
        of(id).pipe(
          withLatestFrom(
            this.store.select(FeatureAirportsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([id, event]) => {
        return this.airportDas
          .delete({
            id: id,
            offlineMode: airportCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (airportCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAirportsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureAirportsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAirportsActions.multiRemove),
      map(x => x?.ids),
      concatMap((ids: number[]) =>
        of(ids).pipe(
          withLatestFrom(
            this.store.select(FeatureAirportsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([ids, event]) => {
        return this.airportDas
          .deletes({
            ids: ids,
            offlineMode: airportCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (airportCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAirportsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureAirportsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private airportDas: AirportDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
