import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  BiaDataChangeService,
  BiaMessageService,
  BiaOnlineOfflineService,
} from 'packages/bia-ng/core/public-api';
import { of } from 'rxjs';
import { catchError, filter, map, switchMap } from 'rxjs/operators';
import { enableSignalrRefresh, storeKey } from '../plane-type-option.constants';
import { PlaneTypeOptionDas } from '../services/plane-type-option-das.service';
import { DomainPlaneTypeOptionsActions } from './plane-type-options-actions';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class PlaneTypeOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(
        DomainPlaneTypeOptionsActions.loadAll
      ) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the PlaneTypes Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'PlaneTypes Reducers' will take care of the rest */
      filter(
        () =>
          enableSignalrRefresh !== true ||
          this.dataChangeService.needsReload(storeKey) === true
      ),
      switchMap(() =>
        this.planeTypeOptionDas
          .getList({
            endpoint: 'allOptions',
            offlineMode: BiaOnlineOfflineService.isModeEnabled,
          })
          .pipe(
            map(planeTypes => {
              this.dataChangeService.markReloaded(storeKey);
              return DomainPlaneTypeOptionsActions.loadAllSuccess({
                planeTypes: planeTypes?.sort((a, b) =>
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
              return of(DomainPlaneTypeOptionsActions.failure({ error: err }));
            })
          )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private planeTypeOptionDas: PlaneTypeOptionDas,
    private biaMessageService: BiaMessageService,
    private dataChangeService: BiaDataChangeService
  ) {}
}
