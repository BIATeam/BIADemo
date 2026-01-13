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
import { Pilot } from '../model/pilot';
import { pilotCRUDConfiguration } from '../pilot.constants';
import { PilotDas } from '../services/pilot-das.service';
import { FeaturePilotsStore } from './pilot.state';
import { FeaturePilotsActions } from './pilots-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class PilotsEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePilotsActions.loadAllByPost),
      map(x => x?.event),
      switchMap(event =>
        this.pilotDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<Pilot[]>) =>
            FeaturePilotsActions.loadAllByPostSuccess({
              result: result,
              event: event,
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeaturePilotsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePilotsActions.load),
      map(x => x?.id),
      switchMap(id => {
        return this.pilotDas.get({ id: id }).pipe(
          map(pilot => FeaturePilotsActions.loadSuccess({ pilot })),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeaturePilotsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePilotsActions.create),
      map(x => {
        return { ...x.pilot, id: undefined };
      }),
      concatMap(pilot =>
        of(pilot).pipe(
          withLatestFrom(
            this.store.select(FeaturePilotsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([pilot, event]) => {
        return this.pilotDas
          .post({
            item: pilot,
            offlineMode: pilotCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              if (pilotCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeaturePilotsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeaturePilotsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePilotsActions.update),
      map(x => x?.pilot),
      concatMap(pilot =>
        of(pilot).pipe(
          withLatestFrom(
            this.store.select(FeaturePilotsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([pilot, event]) => {
        return this.pilotDas
          .put({
            item: pilot,
            id: pilot.id,
            offlineMode: pilotCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (pilotCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeaturePilotsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeaturePilotsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  save$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePilotsActions.save),
      map(x => {
        return x.pilots.map((pilot: Pilot) =>
          pilot.id ? pilot : { ...pilot, id: undefined }
        );
      }),
      concatMap(pilots =>
        of(pilots).pipe(
          withLatestFrom(
            this.store.select(FeaturePilotsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([pilots, event]) => {
        return this.pilotDas
          .save({
            items: pilots,
            offlineMode: pilotCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (pilotCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeaturePilotsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeaturePilotsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePilotsActions.remove),
      map(x => x?.id),
      concatMap((id: number) =>
        of(id).pipe(
          withLatestFrom(
            this.store.select(FeaturePilotsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([id, event]) => {
        return this.pilotDas
          .delete({
            id: id,
            offlineMode: pilotCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (pilotCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeaturePilotsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeaturePilotsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePilotsActions.multiRemove),
      map(x => x?.ids),
      concatMap((ids: number[]) =>
        of(ids).pipe(
          withLatestFrom(
            this.store.select(FeaturePilotsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([ids, event]) => {
        return this.pilotDas
          .deletes({
            ids: ids,
            offlineMode: pilotCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (pilotCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeaturePilotsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeaturePilotsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  updateFixedStatus$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeaturePilotsActions.updateFixedStatus),
      map(x => x),
      concatMap(x =>
        of(x).pipe(
          withLatestFrom(
            this.store.select(FeaturePilotsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([x, event]) => {
        return this.pilotDas
          .updateFixedStatus({ id: x.id, fixed: x.isFixed })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              this.store.dispatch(FeaturePilotsActions.load({ id: x.id }));
              return FeaturePilotsActions.loadAllByPost({ event: event });
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeaturePilotsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private pilotDas: PilotDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
