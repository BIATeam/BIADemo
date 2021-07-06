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
} from './sites-actions';
import { SiteDas } from '../services/site-das.service';
import { SiteInfo } from '../model/site/site-info';
import { SiteInfoDas } from '../services/site-info-das.service';
import { Store } from '@ngrx/store';
import { getLastLazyLoadEvent } from './site.state';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */
@Injectable()
export class SitesEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.siteInfoDas.getListByPost(event).pipe(
          map((result: DataResult<SiteInfo[]>) => loadAllByPostSuccess({ result: result, event: event })),
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
        return this.siteDas.get(id).pipe(
          map((site) => loadSuccess({ site })),
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
      pluck('site'),
      concatMap((site) => of(site).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([site, event]) => {
        return this.siteDas.post(site).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            return loadAllByPost({ event: <LazyLoadEvent>event });
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
      pluck('site'),
      concatMap((site) => of(site).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([site, event]) => {
        return this.siteDas.put(site, site.id).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return loadAllByPost({ event: <LazyLoadEvent>event });
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
        return this.siteDas.delete(id).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return loadAllByPost({ event: <LazyLoadEvent>event });
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
        return this.siteDas.deletes(ids).pipe(
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
    private siteDas: SiteDas,
    private siteInfoDas: SiteInfoDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
