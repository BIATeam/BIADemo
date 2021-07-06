import { createAction, props } from '@ngrx/store';
import { LdapDomain } from '../model/ldap-domain';

export const loadAll = createAction('[Domain LDAP Domains] Load all');
export const loadAllSuccess = createAction('[Domain LDAP Domains] Load all success', props<{ ldapDomains: LdapDomain[] }>());
export const failure = createAction('[Domain LDAP Domains] Failure', props<{ error: any }>());
