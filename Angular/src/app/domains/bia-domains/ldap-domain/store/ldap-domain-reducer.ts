import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { LdapDomain } from '../model/ldap-domain';
import { DomainLdapDomainsActions } from './ldap-domain-actions';

// This adapter will allow is to manipulate LDAP domains (mostly CRUD operations)
export const ldapDomainsAdapter = createEntityAdapter<LdapDomain>({
  selectId: (domain: LdapDomain) => domain.ldapName,
  sortComparer: false,
});

export type State = EntityState<LdapDomain>;

export const INIT_STATE: State = ldapDomainsAdapter.getInitialState({
  // additional props default values here
});

export const ldapDomainReducers = createReducer<State>(
  INIT_STATE,
  on(DomainLdapDomainsActions.loadAllSuccess, (state, { ldapDomains }) =>
    ldapDomainsAdapter.setAll(ldapDomains, state)
  )
);
