import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { DomainCountryOptionsActions } from './country-options-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { CountryOptionDas } from '../services/country-option-das.service';
import { BiaOnlineOfflineService } from 'src/app/core/bia-core/services/bia-online-offline.service';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class CountryOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(
        DomainCountryOptionsActions.loadAll
      ) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the Countries Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'Countries Reducers' will take care of the rest */
      switchMap(() =>
        this.countryDas
          .getList({
            endpoint: 'allOptions',
            offlineMode: BiaOnlineOfflineService.isModeEnabled,
          })
          .pipe(
            map(countries =>
              DomainCountryOptionsActions.loadAllSuccess({
                countries: countries?.sort((a, b) =>
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
              return of(DomainCountryOptionsActions.failure({ error: err }));
            })
          )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private countryDas: CountryOptionDas,
    private biaMessageService: BiaMessageService
  ) {}
}
