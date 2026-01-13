import { inject, Injectable } from '@angular/core';
import {
  BiaMessageService,
  BiaOnlineOfflineService,
} from '@bia-team/bia-ng/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { AnnouncementTypeOptionDas } from '../services/announcement-type-option-das.service';
import { DomainAnnouncementTypeOptionsActions } from './announcement-type-options-actions';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class AnnouncementTypeOptionsEffects {
  protected actions$: Actions = inject(Actions);
  protected announcementTypeOptionDas: AnnouncementTypeOptionDas = inject(
    AnnouncementTypeOptionDas
  );
  protected biaMessageService: BiaMessageService = inject(BiaMessageService);

  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(
        DomainAnnouncementTypeOptionsActions.loadAll
      ) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the AnnouncementTypes Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'AnnouncementTypes Reducers' will take care of the rest */
      switchMap(() =>
        this.announcementTypeOptionDas
          .getList({
            endpoint: 'allOptions',
            offlineMode: BiaOnlineOfflineService.isModeEnabled,
          })
          .pipe(
            map(announcementTypes =>
              DomainAnnouncementTypeOptionsActions.loadAllSuccess({
                announcementTypes: announcementTypes?.sort((a, b) =>
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
                DomainAnnouncementTypeOptionsActions.failure({ error: err })
              );
            })
          )
      )
    )
  );
}
