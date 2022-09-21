import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap, withLatestFrom, concatMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { FeatureSitesActions } from './sites-actions';
import { Store } from '@ngrx/store';
import { FeatureSitesStore } from './site.state';
import { Site } from '../model/site';
import { SiteCRUDConfiguration } from '../site.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { SiteDas } from '../services/site-das.service';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class SitesEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureSitesActions.loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.siteDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<Site[]>) => FeatureSitesActions.loadAllByPostSuccess({ result: result, event: event })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureSitesActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureSitesActions.load),
      pluck('id'),
      switchMap((id) => {
        return this.siteDas.get({ id: id }).pipe(
          map((site) => FeatureSitesActions.loadSuccess({ site })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureSitesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureSitesActions.create),
      pluck('site'),
      concatMap((site) => of(site).pipe(withLatestFrom(this.store.select(FeatureSitesStore.getLastLazyLoadEvent)))),
      switchMap(([site, event]) => {
        return this.siteDas.post({ item: site, offlineMode: SiteCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            if (SiteCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureSitesActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureSitesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureSitesActions.update),
      pluck('site'),
      concatMap((site) => of(site).pipe(withLatestFrom(this.store.select(FeatureSitesStore.getLastLazyLoadEvent)))),
      switchMap(([site, event]) => {
        return this.siteDas.put({ item: site, id: site.id, offlineMode: SiteCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            if (SiteCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureSitesActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureSitesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureSitesActions.remove),
      pluck('id'),
      concatMap((id: number) => of(id).pipe(withLatestFrom(this.store.select(FeatureSitesStore.getLastLazyLoadEvent)))),
      switchMap(([id, event]) => {
        return this.siteDas.delete({ id: id, offlineMode: SiteCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (SiteCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureSitesActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureSitesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureSitesActions.multiRemove),
      pluck('ids'),
      concatMap((ids: number[]) => of(ids).pipe(withLatestFrom(this.store.select(FeatureSitesStore.getLastLazyLoadEvent)))),
      switchMap(([ids, event]) => {
        return this.siteDas.deletes({ ids: ids, offlineMode: SiteCRUDConfiguration.useOfflineMode }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (SiteCRUDConfiguration.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureSitesActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureSitesActions.failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private siteDas: SiteDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
