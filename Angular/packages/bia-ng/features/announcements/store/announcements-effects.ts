import { inject, Injectable } from '@angular/core';
import {
  BiaMessageService,
  biaSuccessWaitRefreshSignalR,
} from '@bia-team/bia-ng/core';
import { Announcement, DataResult } from '@bia-team/bia-ng/models';
import { BiaAppState } from '@bia-team/bia-ng/store';
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
import { announcementCRUDConfiguration } from '../announcement.constants';
import { AnnouncementDas } from '../services/announcement-das.service';
import { FeatureAnnouncementsStore } from './announcement.state';
import { FeatureAnnouncementsActions } from './announcements-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class AnnouncementsEffects {
  protected actions$: Actions = inject(Actions);
  protected announcementDas: AnnouncementDas = inject(AnnouncementDas);
  protected biaMessageService: BiaMessageService = inject(BiaMessageService);
  protected store: Store<BiaAppState> = inject(Store<BiaAppState>);

  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAnnouncementsActions.loadAllByPost),
      map(x => x?.event),
      switchMap(event =>
        this.announcementDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<Announcement[]>) =>
            FeatureAnnouncementsActions.loadAllByPostSuccess({
              result: result,
              event: event,
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureAnnouncementsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAnnouncementsActions.load),
      map(x => x?.id),
      switchMap(id => {
        return this.announcementDas.get({ id: id }).pipe(
          map(announcement => {
            if (announcementCRUDConfiguration.displayHistorical) {
              this.store.dispatch(
                FeatureAnnouncementsActions.loadHistorical({
                  id: announcement.id,
                })
              );
            }
            return FeatureAnnouncementsActions.loadSuccess({ announcement });
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureAnnouncementsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAnnouncementsActions.create),
      map(x => x?.announcement),
      concatMap(announcement =>
        of(announcement).pipe(
          withLatestFrom(
            this.store.select(FeatureAnnouncementsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([announcement, event]) => {
        return this.announcementDas
          .post({
            item: announcement,
            offlineMode: announcementCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              if (announcementCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAnnouncementsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureAnnouncementsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAnnouncementsActions.update),
      map(x => x?.announcement),
      concatMap(announcement =>
        of(announcement).pipe(
          withLatestFrom(
            this.store.select(FeatureAnnouncementsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([announcement, event]) => {
        return this.announcementDas
          .put({
            item: announcement,
            id: announcement.id,
            offlineMode: announcementCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (announcementCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAnnouncementsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureAnnouncementsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAnnouncementsActions.remove),
      map(x => x?.id),
      concatMap((id: number) =>
        of(id).pipe(
          withLatestFrom(
            this.store.select(FeatureAnnouncementsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([id, event]) => {
        return this.announcementDas
          .delete({
            id: id,
            offlineMode: announcementCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (announcementCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAnnouncementsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureAnnouncementsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAnnouncementsActions.multiRemove),
      map(x => x?.ids),
      concatMap((ids: number[]) =>
        of(ids).pipe(
          withLatestFrom(
            this.store.select(FeatureAnnouncementsStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([ids, event]) => {
        return this.announcementDas
          .deletes({
            ids: ids,
            offlineMode: announcementCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (announcementCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAnnouncementsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureAnnouncementsActions.failure({ error: err }));
            })
          );
      })
    )
  );

  loadHistorical$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAnnouncementsActions.loadHistorical),
      map(x => x?.id),
      switchMap(id => {
        return this.announcementDas.getHistorical({ id: id }).pipe(
          map(historical => {
            return FeatureAnnouncementsActions.loadHistoricalSuccess({
              historical,
            });
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureAnnouncementsActions.failure({ error: err }));
          })
        );
      })
    )
  );
}
