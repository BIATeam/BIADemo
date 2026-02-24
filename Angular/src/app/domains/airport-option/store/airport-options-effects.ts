import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  BiaDataChangeService,
  BiaMessageService,
  BiaOnlineOfflineService,
} from 'packages/bia-ng/core/public-api';
import { of } from 'rxjs';
import { catchError, filter, map, switchMap } from 'rxjs/operators';
import { enableSignalrRefresh, storeKey } from '../airport-option.contants';
import { AirportOptionDas } from '../services/airport-option-das.service';
import { DomainAirportOptionsActions } from './airport-options-actions';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class AirportOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(
        DomainAirportOptionsActions.loadAll
      ) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the Airports Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'Airports Reducers' will take care of the rest */
      filter(
        () =>
          enableSignalrRefresh !== true ||
          this.dataChangeService.needsReload(storeKey) === true
      ),
      switchMap(() =>
        this.airportOptionDas
          .getList({
            endpoint: 'allOptions',
            offlineMode: BiaOnlineOfflineService.isModeEnabled,
          })
          .pipe(
            map(airports => {
              this.dataChangeService.markReloaded(storeKey);
              return DomainAirportOptionsActions.loadAllSuccess({
                airports: airports?.sort((a, b) =>
                  a.display.localeCompare(b.display)
                ),
              });
            }),
            catchError(err => {
              if (
                BiaOnlineOfflineService.isModeEnabled !== true ||
                BiaOnlineOfflineService.isServerAvailable(err) === true
              ) {
                this.biaMessageService.showErrorHttpResponse(err);
              }
              return of(DomainAirportOptionsActions.failure({ error: err }));
            })
          )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private airportOptionDas: AirportOptionDas,
    private biaMessageService: BiaMessageService,
    private dataChangeService: BiaDataChangeService
  ) {}
}
