import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { DomainLanguageOptionsActions } from './language-options-actions';
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
      ofType(
        DomainLanguageOptionsActions.loadAll
      ) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the Languages Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'Languages Reducers' will take care of the rest */
      switchMap(() =>
        this.languageDas.getList({ endpoint: 'allOptions' }).pipe(
          map(languages =>
            DomainLanguageOptionsActions.loadAllSuccess({
              languages: languages?.sort((a, b) =>
                a.display.localeCompare(b.display)
              ),
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(DomainLanguageOptionsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  constructor(
    protected actions$: Actions,
    protected languageDas: LanguageOptionDas,
    protected biaMessageService: BiaMessageService
  ) {}
}
