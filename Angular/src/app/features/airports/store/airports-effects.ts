import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap, withLatestFrom, concatMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { FeatureAirportsActions } from './airports-actions';
import { Store } from '@ngrx/store';
import { FeatureAirportsStore } from './airport.state';
import { Airport } from '../model/airport';
import { AirportCRUDConfiguration } from '../airport.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { AirportDas } from '../services/airport-das.service';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class AirportsEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAirportsActions.loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.airportDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<Airport[]>) => FeatureAirportsActions.loadAllByPostSuccess({ result: result, event: event })),
          catchError((err) => {
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
      pluck('id'),
      switchMap((id) => {
        if (id) {
          return this.airportDas.get({ id: id }).pipe(
            map((airport) => FeatureAirportsActions.loadSuccess({ airport })),
            catchError((err) => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureAirportsActions.failure({ error: err }));
            })
          );
        } else {
          return of(FeatureAirportsActions.loadSuccess({ airport: <Airport>{} }));
        }
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAirportsActions.create),
      pluck('airport'),
      concatMap((airport) => of(airport).pipe(withLatestFrom(this.store.select(FeatureAirportsStore.getLastLazyLoadEvent)))),
      switchMap(([airport, event]) => {
        return this.airportDas.post({ item: airport, offlineMode: AirportCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            if (AirportCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureAirportsActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
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
      pluck('airport'),
      concatMap((airport) => of(airport).pipe(withLatestFrom(this.store.select(FeatureAirportsStore.getLastLazyLoadEvent)))),
      switchMap(([airport, event]) => {
        return this.airportDas.put({ item: airport, id: airport.id, offlineMode: AirportCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            if (AirportCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureAirportsActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
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
      pluck('id'),
      concatMap((id: number) => of(id).pipe(withLatestFrom(this.store.select(FeatureAirportsStore.getLastLazyLoadEvent)))),
      switchMap(([id, event]) => {
        return this.airportDas.delete({ id: id, offlineMode: AirportCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (AirportCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureAirportsActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
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
      pluck('ids'),
      concatMap((ids: number[]) => of(ids).pipe(withLatestFrom(this.store.select(FeatureAirportsStore.getLastLazyLoadEvent)))),
      switchMap(([ids, event]) => {
        return this.airportDas.deletes({ ids: ids, offlineMode: AirportCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (AirportCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureAirportsActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
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
  ) { }
}
