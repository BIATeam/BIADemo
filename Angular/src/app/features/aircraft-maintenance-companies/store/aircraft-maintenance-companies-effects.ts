import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import {
  catchError,
  map,
  switchMap,
  withLatestFrom,
  concatMap,
} from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { FeatureAircraftMaintenanceCompaniesActions } from './aircraft-maintenance-companies-actions';
import { Store } from '@ngrx/store';
import { FeatureAircraftMaintenanceCompaniesStore } from './aircraft-maintenance-company.state';
import { AircraftMaintenanceCompany } from '../model/aircraft-maintenance-company';
import { aircraftMaintenanceCompanyCRUDConfiguration } from '../aircraft-maintenance-company.constants';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { AircraftMaintenanceCompanyDas } from '../services/aircraft-maintenance-company-das.service';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class AircraftMaintenanceCompaniesEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAircraftMaintenanceCompaniesActions.loadAllByPost),
      map(x => x?.event),
      switchMap(event =>
        this.aircraftMaintenanceCompanyDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<AircraftMaintenanceCompany[]>) =>
            FeatureAircraftMaintenanceCompaniesActions.loadAllByPostSuccess({
              result: result,
              event: event,
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(
              FeatureAircraftMaintenanceCompaniesActions.failure({ error: err })
            );
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAircraftMaintenanceCompaniesActions.load),
      map(x => x?.id),
      switchMap(id => {
        return this.aircraftMaintenanceCompanyDas.get({ id: id }).pipe(
          map(aircraftMaintenanceCompany =>
            FeatureAircraftMaintenanceCompaniesActions.loadSuccess({
              aircraftMaintenanceCompany,
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(
              FeatureAircraftMaintenanceCompaniesActions.failure({
                error: err,
              })
            );
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAircraftMaintenanceCompaniesActions.create),
      map(x => x?.aircraftMaintenanceCompany),
      concatMap(aircraftMaintenanceCompany =>
        of(aircraftMaintenanceCompany).pipe(
          withLatestFrom(
            this.store.select(
              FeatureAircraftMaintenanceCompaniesStore.getLastLazyLoadEvent
            )
          )
        )
      ),
      switchMap(([aircraftMaintenanceCompany, event]) => {
        return this.aircraftMaintenanceCompanyDas
          .post({
            item: aircraftMaintenanceCompany,
            offlineMode:
              aircraftMaintenanceCompanyCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              if (aircraftMaintenanceCompanyCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAircraftMaintenanceCompaniesActions.loadAllByPost(
                  { event: <LazyLoadEvent>event }
                );
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(
                FeatureAircraftMaintenanceCompaniesActions.failure({
                  error: err,
                })
              );
            })
          );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAircraftMaintenanceCompaniesActions.update),
      map(x => x?.aircraftMaintenanceCompany),
      concatMap(aircraftMaintenanceCompany =>
        of(aircraftMaintenanceCompany).pipe(
          withLatestFrom(
            this.store.select(
              FeatureAircraftMaintenanceCompaniesStore.getLastLazyLoadEvent
            )
          )
        )
      ),
      switchMap(([aircraftMaintenanceCompany, event]) => {
        return this.aircraftMaintenanceCompanyDas
          .put({
            item: aircraftMaintenanceCompany,
            id: aircraftMaintenanceCompany.id,
            offlineMode:
              aircraftMaintenanceCompanyCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (aircraftMaintenanceCompanyCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAircraftMaintenanceCompaniesActions.loadAllByPost(
                  { event: <LazyLoadEvent>event }
                );
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(
                FeatureAircraftMaintenanceCompaniesActions.failure({
                  error: err,
                })
              );
            })
          );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAircraftMaintenanceCompaniesActions.remove),
      map(x => x?.id),
      concatMap((id: number) =>
        of(id).pipe(
          withLatestFrom(
            this.store.select(
              FeatureAircraftMaintenanceCompaniesStore.getLastLazyLoadEvent
            )
          )
        )
      ),
      switchMap(([id, event]) => {
        return this.aircraftMaintenanceCompanyDas
          .delete({
            id: id,
            offlineMode:
              aircraftMaintenanceCompanyCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (aircraftMaintenanceCompanyCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAircraftMaintenanceCompaniesActions.loadAllByPost(
                  { event: <LazyLoadEvent>event }
                );
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(
                FeatureAircraftMaintenanceCompaniesActions.failure({
                  error: err,
                })
              );
            })
          );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureAircraftMaintenanceCompaniesActions.multiRemove),
      map(x => x?.ids),
      concatMap((ids: number[]) =>
        of(ids).pipe(
          withLatestFrom(
            this.store.select(
              FeatureAircraftMaintenanceCompaniesStore.getLastLazyLoadEvent
            )
          )
        )
      ),
      switchMap(([ids, event]) => {
        return this.aircraftMaintenanceCompanyDas
          .deletes({
            ids: ids,
            offlineMode:
              aircraftMaintenanceCompanyCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (aircraftMaintenanceCompanyCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureAircraftMaintenanceCompaniesActions.loadAllByPost(
                  { event: <LazyLoadEvent>event }
                );
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(
                FeatureAircraftMaintenanceCompaniesActions.failure({
                  error: err,
                })
              );
            })
          );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private aircraftMaintenanceCompanyDas: AircraftMaintenanceCompanyDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
