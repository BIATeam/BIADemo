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
import { flightCRUDConfiguration } from '../flight.constants';
import { Flight } from '../model/flight';
import { FlightDas } from '../services/flight-das.service';
import { FeatureFlightsStore } from './flight.state';
import { FeatureFlightsActions } from './flights-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class FlightsEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureFlightsActions.loadAllByPost),
      map(x => x?.event),
      switchMap(event =>
        this.flightDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<Flight[]>) =>
            FeatureFlightsActions.loadAllByPostSuccess({
              result: result,
              event: event,
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureFlightsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureFlightsActions.load),
      map(x => x?.id),
      switchMap(id => {
        return this.flightDas.get({ id: id }).pipe(
          map(flight => FeatureFlightsActions.loadSuccess({ flight })),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureFlightsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureFlightsActions.create),
      map(x => x?.flight),
      concatMap(flight =>
        of(flight).pipe(
          withLatestFrom(
            this.store.select(FeatureFlightsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([flight, event]) => {
        return this.flightDas
          .post({
            item: flight,
            offlineMode: flightCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              if (flightCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureFlightsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureFlightsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureFlightsActions.update),
      map(x => x?.flight),
      concatMap(flight =>
        of(flight).pipe(
          withLatestFrom(
            this.store.select(FeatureFlightsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([flight, event]) => {
        return this.flightDas
          .put({
            item: flight,
            id: flight.id,
            offlineMode: flightCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (flightCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureFlightsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureFlightsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  save$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureFlightsActions.save),
      map(x => x?.flights),
      concatMap(flights =>
        of(flights).pipe(
          withLatestFrom(
            this.store.select(FeatureFlightsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([flights, event]) => {
        return this.flightDas
          .save({
            items: flights,
            offlineMode: flightCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (flightCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureFlightsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureFlightsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureFlightsActions.remove),
      map(x => x?.id),
      concatMap((id: number) =>
        of(id).pipe(
          withLatestFrom(
            this.store.select(FeatureFlightsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([id, event]) => {
        return this.flightDas
          .delete({
            id: id,
            offlineMode: flightCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (flightCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureFlightsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureFlightsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureFlightsActions.multiRemove),
      map(x => x?.ids),
      concatMap((ids: number[]) =>
        of(ids).pipe(
          withLatestFrom(
            this.store.select(FeatureFlightsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([ids, event]) => {
        return this.flightDas
          .deletes({
            ids: ids,
            offlineMode: flightCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (flightCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureFlightsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureFlightsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  updateFixedStatus$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureFlightsActions.updateFixedStatus),
      map(x => x),
      concatMap(x =>
        of(x).pipe(
          withLatestFrom(
            this.store.select(FeatureFlightsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([x, event]) => {
        return this.flightDas
          .updateFixedStatus({ id: x.id, fixed: x.isFixed })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              this.store.dispatch(FeatureFlightsActions.load({ id: x.id }));
              return FeatureFlightsActions.loadAllByPost({ event: event });
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureFlightsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private flightDas: FlightDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
