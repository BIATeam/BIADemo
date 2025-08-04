import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { BiaMessageService, biaSuccessWaitRefreshSignalR } from 'bia-ng/core';
import { DataResult } from 'bia-ng/models';
import { of } from 'rxjs';
import {
  catchError,
  concatMap,
  map,
  switchMap,
  withLatestFrom,
} from 'rxjs/operators';
import { AppState } from 'src/app/store/state';
import { maintenanceContractCRUDConfiguration } from '../maintenance-contract.constants';
import { MaintenanceContract } from '../model/maintenance-contract';
import { MaintenanceContractDas } from '../services/maintenance-contract-das.service';
import { FeatureMaintenanceContractsStore } from './maintenance-contract.state';
import { FeatureMaintenanceContractsActions } from './maintenance-contracts-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class MaintenanceContractsEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMaintenanceContractsActions.loadAllByPost),
      map(x => x?.event),
      switchMap(event =>
        this.maintenanceContractDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<MaintenanceContract[]>) =>
            FeatureMaintenanceContractsActions.loadAllByPostSuccess({
              result: result,
              event: event,
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(
              FeatureMaintenanceContractsActions.failure({ error: err })
            );
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMaintenanceContractsActions.load),
      map(x => x?.id),
      switchMap(id => {
        return this.maintenanceContractDas.get({ id: id }).pipe(
          map(maintenanceContract =>
            FeatureMaintenanceContractsActions.loadSuccess({
              maintenanceContract,
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(
              FeatureMaintenanceContractsActions.failure({ error: err })
            );
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMaintenanceContractsActions.create),
      map(x => x?.maintenanceContract),
      concatMap(maintenanceContract =>
        of(maintenanceContract).pipe(
          withLatestFrom(
            this.store.select(
              FeatureMaintenanceContractsStore.getLastLazyLoadEvent
            )
          )
        )
      ),
      switchMap(([maintenanceContract, event]) => {
        return this.maintenanceContractDas
          .post({
            item: maintenanceContract,
            offlineMode: maintenanceContractCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              if (maintenanceContractCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureMaintenanceContractsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(
                FeatureMaintenanceContractsActions.failure({ error: err })
              );
            })
          );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMaintenanceContractsActions.update),
      map(x => x?.maintenanceContract),
      concatMap(maintenanceContract =>
        of(maintenanceContract).pipe(
          withLatestFrom(
            this.store.select(
              FeatureMaintenanceContractsStore.getLastLazyLoadEvent
            )
          )
        )
      ),
      switchMap(([maintenanceContract, event]) => {
        return this.maintenanceContractDas
          .put({
            item: maintenanceContract,
            id: maintenanceContract.id,
            offlineMode: maintenanceContractCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (maintenanceContractCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureMaintenanceContractsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(
                FeatureMaintenanceContractsActions.failure({ error: err })
              );
            })
          );
      })
    )
  );

  save$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMaintenanceContractsActions.save),
      map(x => x?.maintenanceContracts),
      concatMap(maintenanceContracts =>
        of(maintenanceContracts).pipe(
          withLatestFrom(
            this.store.select(
              FeatureMaintenanceContractsStore.getLastLazyLoadEvent
            )
          )
        )
      ),
      switchMap(([maintenanceContracts, event]) => {
        return this.maintenanceContractDas
          .save({
            items: maintenanceContracts,
            offlineMode: maintenanceContractCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (maintenanceContractCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureMaintenanceContractsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(
                FeatureMaintenanceContractsActions.failure({ error: err })
              );
            })
          );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMaintenanceContractsActions.remove),
      map(x => x?.id),
      concatMap((id: number) =>
        of(id).pipe(
          withLatestFrom(
            this.store.select(
              FeatureMaintenanceContractsStore.getLastLazyLoadEvent
            )
          )
        )
      ),
      switchMap(([id, event]) => {
        return this.maintenanceContractDas
          .delete({
            id: id,
            offlineMode: maintenanceContractCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (maintenanceContractCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureMaintenanceContractsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(
                FeatureMaintenanceContractsActions.failure({ error: err })
              );
            })
          );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMaintenanceContractsActions.multiRemove),
      map(x => x?.ids),
      concatMap((ids: number[]) =>
        of(ids).pipe(
          withLatestFrom(
            this.store.select(
              FeatureMaintenanceContractsStore.getLastLazyLoadEvent
            )
          )
        )
      ),
      switchMap(([ids, event]) => {
        return this.maintenanceContractDas
          .deletes({
            ids: ids,
            offlineMode: maintenanceContractCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (maintenanceContractCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureMaintenanceContractsActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(
                FeatureMaintenanceContractsActions.failure({ error: err })
              );
            })
          );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private maintenanceContractDas: MaintenanceContractDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
