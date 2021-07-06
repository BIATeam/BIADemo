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
} from './airports-actions';
import { AirportDas } from '../services/airport-das.service';
import { Store } from '@ngrx/store';
import { getLastLazyLoadEvent } from './airport.state';
import { Airport } from '../model/airport';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class AirportsEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.airportDas.getListByPost(event).pipe(
          map((result: DataResult<Airport[]>) => loadAllByPostSuccess({ result: result, event: event })),
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
        return this.airportDas.get(id).pipe(
          map((airport) => loadSuccess({ airport })),
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
      pluck('airport'),
      concatMap((airport) => of(airport).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([airport, event]) => {
        return this.airportDas.post(airport).pipe(
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
      pluck('airport'),
      concatMap((airport) => of(airport).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([airport, event]) => {
        return this.airportDas.put(airport, airport.id).pipe(
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
        return this.airportDas.delete(id).pipe(
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
        return this.airportDas.deletes(ids).pipe(
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
    private airportDas: AirportDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
