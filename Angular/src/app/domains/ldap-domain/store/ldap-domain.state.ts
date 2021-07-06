import * as fromLdapDomains from './ldap-domain-reducer';

import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';

export interface LdapDomainState {
  ldapDomain: fromLdapDomains.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: LdapDomainState | undefined, action: Action) {
  return combineReducers({
    ldapDomain: fromLdapDomains.ldapDomainReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getUsersState = createFeatureSelector<LdapDomainState>('domain-ldap-domains');

export const getUsersEntitiesState = createSelector(
  getUsersState,
  (state) => state.ldapDomain
);

export const { selectAll: getAllLdapDomain } = fromLdapDomains.ldapDomainsAdapter.getSelectors(getUsersEntitiesState);
