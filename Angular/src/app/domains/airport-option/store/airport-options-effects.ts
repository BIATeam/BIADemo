import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  loadAllAirportOptions,
  loadAllSuccess
} from './airport-options-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { AirportOptionDas } from '../services/airport-option-das.service';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class AirportOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllAirportOptions) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the Airports Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'Airports Reducers' will take care of the rest */
      switchMap(() =>
        this.airportDas.getList('allOptions').pipe(
          map((airports) => loadAllSuccess({ airports })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  /*
  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(load),
      pluck('id'),
      switchMap((id) =>
        this.airportDas.get(id).pipe(
          map((airport) => loadSuccess({ airport })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );
  */

  constructor(
    private actions$: Actions,
    private airportDas: AirportOptionDas,
    private biaMessageService: BiaMessageService
  ) {}
}
