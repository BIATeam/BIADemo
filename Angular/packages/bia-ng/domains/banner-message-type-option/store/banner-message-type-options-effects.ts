import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  BiaMessageService,
  BiaOnlineOfflineService,
} from 'packages/bia-ng/core/public-api';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { BannerMessageTypeOptionDas } from '../services/banner-message-type-option-das.service';
import { DomainBannerMessageTypeOptionsActions } from './banner-message-type-options-actions';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class BannerMessageTypeOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(
        DomainBannerMessageTypeOptionsActions.loadAll
      ) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the BannerMessageTypes Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'BannerMessageTypes Reducers' will take care of the rest */
      switchMap(() =>
        this.bannerMessageTypeOptionDas
          .getList({
            endpoint: 'allOptions',
            offlineMode: BiaOnlineOfflineService.isModeEnabled,
          })
          .pipe(
            map(bannerMessageTypes =>
              DomainBannerMessageTypeOptionsActions.loadAllSuccess({
                bannerMessageTypes: bannerMessageTypes?.sort((a, b) =>
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
              return of(
                DomainBannerMessageTypeOptionsActions.failure({ error: err })
              );
            })
          )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private bannerMessageTypeOptionDas: BannerMessageTypeOptionDas,
    private biaMessageService: BiaMessageService
  ) {}
}
