import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaOnlineOfflineService } from 'src/app/core/bia-core/services/bia-online-offline.service';
import { AircraftMaintenanceCompanyOptionDas } from '../services/aircraft-maintenance-company-option-das.service';
import { DomainAircraftMaintenanceCompanyOptionsActions } from './aircraft-maintenance-company-options-actions';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class AircraftMaintenanceCompanyOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(
        DomainAircraftMaintenanceCompanyOptionsActions.loadAll
      ) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the AircraftMaintenanceCompanies Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'AircraftMaintenanceCompanies Reducers' will take care of the rest */
      switchMap(() =>
        this.aircraftMaintenanceCompanyDas
          .getList({
            endpoint: 'allOptions',
            offlineMode: BiaOnlineOfflineService.isModeEnabled,
          })
          .pipe(
            map(aircraftMaintenanceCompanies =>
              DomainAircraftMaintenanceCompanyOptionsActions.loadAllSuccess({
                aircraftMaintenanceCompanies: aircraftMaintenanceCompanies?.sort((a, b) =>
                  a.display.localeCompare(b.display)
                ),
              })
            ),
            catchError(err => {
              if (
                BiaOnlineOfflineService.isModeEnabled !== true ||
                BiaOnlineOfflineService.isServerAvailable(err) === true
              ) {
                this.biaMessageService.showErrorHttpResponse(err);
              }
              return of(DomainAircraftMaintenanceCompanyOptionsActions.failure({ error: err }));
            })
          )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private aircraftMaintenanceCompanyDas: AircraftMaintenanceCompanyOptionDas,
    private biaMessageService: BiaMessageService
  ) {}
}
