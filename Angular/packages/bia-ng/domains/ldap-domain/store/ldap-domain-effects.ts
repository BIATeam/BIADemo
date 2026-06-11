import { inject, Injectable } from '@angular/core';
import { BiaMessageService } from '@bia-team/bia-ng/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { LdapDomainDas } from '../services/ldap-domain-das.service';
import { DomainLdapDomainsActions } from './ldap-domain-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class LdapDomainsEffects {
  protected actions$: Actions = inject(Actions);
  protected ldapDomainDas: LdapDomainDas = inject(LdapDomainDas);
  protected biaMessageService: BiaMessageService = inject(BiaMessageService);

  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainLdapDomainsActions.loadAll) /* When action is dispatched */,
      switchMap(() => {
        return this.ldapDomainDas.getAll().pipe(
          map(ldapDomains =>
            DomainLdapDomainsActions.loadAllSuccess({ ldapDomains })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(DomainLdapDomainsActions.failure({ error: err }));
          })
        );
      })
    )
  );
}
