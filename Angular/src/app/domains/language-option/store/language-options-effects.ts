import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  loadAllLanguageOptions,
  loadAllSuccess
} from './language-options-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LanguageOptionDas } from '../services/language-option-das.service';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class LanguageOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllLanguageOptions) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the Languages Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'Languages Reducers' will take care of the rest */
      switchMap(() =>
        this.languageDas.getList('allOptions').pipe(
          map((languages) => loadAllSuccess({ languages })),
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
        this.languageDas.get(id).pipe(
          map((language) => loadSuccess({ language })),
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
    private languageDas: LanguageOptionDas,
    private biaMessageService: BiaMessageService
  ) {}
}
