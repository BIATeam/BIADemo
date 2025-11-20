import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import {
  BiaMessageService,
  biaSuccessWaitRefreshSignalR,
} from 'packages/bia-ng/core/public-api';
import { DataResult } from 'packages/bia-ng/models/public-api';
import { of } from 'rxjs';
import {
  catchError,
  concatMap,
  map,
  switchMap,
  withLatestFrom,
} from 'rxjs/operators';
import { AppState } from 'src/app/store/state';
import { annoucementCRUDConfiguration } from '../annoucement.constants';
import { Annoucement } from '../model/annoucement';
import { AnnoucementDas } from '../services/annoucement-das.service';
import { FeatureAnnoucementsStore } from './annoucement.state';
import { FeatureAnnoucementsActions } from './annoucements-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class AnnoucementsEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAnnoucementsActions.loadAllByPost),
      map(x => x?.event),
      switchMap(event =>
        this.annoucementDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<Annoucement[]>) =>
            FeatureAnnoucementsActions.loadAllByPostSuccess({
              result: result,
              event: event,
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureAnnoucementsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAnnoucementsActions.load),
      map(x => x?.id),
      switchMap(id => {
        return this.annoucementDas.get({ id: id }).pipe(
          map(annoucement => {
            if (annoucementCRUDConfiguration.displayHistorical) {
              this.store.dispatch(
                FeatureAnnoucementsActions.loadHistorical({
                  id: annoucement.id,
                })
              );
            }
            return FeatureAnnoucementsActions.loadSuccess({ annoucement });
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureAnnoucementsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAnnoucementsActions.create),
      map(x => x?.annoucement),
      concatMap(annoucement =>
        of(annoucement).pipe(
          withLatestFrom(
            this.store.select(FeatureAnnoucementsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([annoucement, event]) => {
        return this.annoucementDas
          .post({
            item: annoucement,
            offlineMode: annoucementCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              if (annoucementCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAnnoucementsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureAnnoucementsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAnnoucementsActions.update),
      map(x => x?.annoucement),
      concatMap(annoucement =>
        of(annoucement).pipe(
          withLatestFrom(
            this.store.select(FeatureAnnoucementsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([annoucement, event]) => {
        return this.annoucementDas
          .put({
            item: annoucement,
            id: annoucement.id,
            offlineMode: annoucementCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (annoucementCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAnnoucementsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureAnnoucementsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAnnoucementsActions.remove),
      map(x => x?.id),
      concatMap((id: number) =>
        of(id).pipe(
          withLatestFrom(
            this.store.select(FeatureAnnoucementsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([id, event]) => {
        return this.annoucementDas
          .delete({
            id: id,
            offlineMode: annoucementCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (annoucementCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAnnoucementsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureAnnoucementsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAnnoucementsActions.multiRemove),
      map(x => x?.ids),
      concatMap((ids: number[]) =>
        of(ids).pipe(
          withLatestFrom(
            this.store.select(FeatureAnnoucementsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([ids, event]) => {
        return this.annoucementDas
          .deletes({
            ids: ids,
            offlineMode: annoucementCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (annoucementCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAnnoucementsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureAnnoucementsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  loadHistorical$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAnnoucementsActions.loadHistorical),
      map(x => x?.id),
      switchMap(id => {
        return this.annoucementDas.getHistorical({ id: id }).pipe(
          map(historical => {
            return FeatureAnnoucementsActions.loadHistoricalSuccess({
              historical,
            });
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureAnnoucementsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private annoucementDas: AnnoucementDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
