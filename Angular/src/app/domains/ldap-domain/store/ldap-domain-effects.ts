import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { failure, loadAll, loadAllSuccess } from './ldap-domain-actions';
import { LdapDomainDas } from '../services/ldap-domain-das.service';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class LdapDomainsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAll) /* When action is dispatched */,
      switchMap((action) => {
        return this.ldapDomainDas.getAll().pipe(
          map((ldapDomains) => loadAllSuccess({ ldapDomains })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private ldapDomainDas: LdapDomainDas,
    private biaMessageService: BiaMessageService
  ) {}
}
