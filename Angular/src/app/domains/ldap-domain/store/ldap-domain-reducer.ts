import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { LdapDomain } from '../model/ldap-domain';
import { loadAllSuccess } from './ldap-domain-actions';

// This adapter will allow is to manipulate LDAP domains (mostly CRUD operations)
export const ldapDomainsAdapter = createEntityAdapter<LdapDomain>({
  selectId: (domain: LdapDomain) => domain.ldapName,
  sortComparer: false
});

export interface State extends EntityState<LdapDomain> {
  // additional props here
}

export const INIT_STATE: State = ldapDomainsAdapter.getInitialState({
  // additional props default values here
});

export const ldapDomainReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllSuccess, (state, { ldapDomains }) => ldapDomainsAdapter.setAll(ldapDomains, state))
);
