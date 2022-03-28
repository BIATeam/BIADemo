import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap, withLatestFrom, concatMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { FeatureMaintenanceTeamsActions } from './maintenance-teams-actions';
import { MaintenanceTeamDas } from '../services/maintenance-team-das.service';
import { Store } from '@ngrx/store';
import { getLastLazyLoadEvent } from './maintenance-team.state';
import { MaintenanceTeam } from '../model/maintenance-team';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { useSignalR } from '../maintenance-team.constants';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class MaintenanceTeamsEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMaintenanceTeamsActions.loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.maintenanceTeamDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<MaintenanceTeam[]>) => FeatureMaintenanceTeamsActions.loadAllByPostSuccess({ result: result, event: event })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureMaintenanceTeamsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMaintenanceTeamsActions.load),
      pluck('id'),
      switchMap((id) => {
        return this.maintenanceTeamDas.get({ id: id }).pipe(
          map((maintenanceTeam) => FeatureMaintenanceTeamsActions.loadSuccess({ maintenanceTeam })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureMaintenanceTeamsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMaintenanceTeamsActions.create),
      pluck('maintenanceTeam'),
      concatMap((maintenanceTeam) => of(maintenanceTeam).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([maintenanceTeam, event]) => {
        return this.maintenanceTeamDas.post({ item: maintenanceTeam }).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            if (useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureMaintenanceTeamsActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureMaintenanceTeamsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMaintenanceTeamsActions.update),
      pluck('maintenanceTeam'),
      concatMap((maintenanceTeam) => of(maintenanceTeam).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([maintenanceTeam, event]) => {
        return this.maintenanceTeamDas.put({ item: maintenanceTeam, id: maintenanceTeam.id }).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            if (useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureMaintenanceTeamsActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureMaintenanceTeamsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMaintenanceTeamsActions.remove),
      pluck('id'),
      concatMap((id: number) => of(id).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([id, event]) => {
        return this.maintenanceTeamDas.delete({ id: id }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureMaintenanceTeamsActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureMaintenanceTeamsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMaintenanceTeamsActions.multiRemove),
      pluck('ids'),
      concatMap((ids: number[]) => of(ids).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([ids, event]) => {
        return this.maintenanceTeamDas.deletes({ ids: ids }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureMaintenanceTeamsActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureMaintenanceTeamsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private maintenanceTeamDas: MaintenanceTeamDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) { }
}
