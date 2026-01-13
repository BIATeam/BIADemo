import { LdapDomain } from '@bia-team/bia-ng/models';
import { createAction, props } from '@ngrx/store';

export namespace DomainLdapDomainsActions {
  export const loadAll = createAction('[Domain LDAP Domains] Load all');
  export const loadAllSuccess = createAction(
    '[Domain LDAP Domains] Load all success',
    props<{ ldapDomains: LdapDomain[] }>()
  );
  export const failure = createAction(
    '[Domain LDAP Domains] Failure',
    props<{ error: any }>()
  );
}
