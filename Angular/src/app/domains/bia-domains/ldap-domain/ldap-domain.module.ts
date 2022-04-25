import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/ldap-domain.state';
import { LdapDomainsEffects } from './store/ldap-domain-effects';

@NgModule({
  imports: [StoreModule.forFeature('domain-ldap-domains', reducers), EffectsModule.forFeature([LdapDomainsEffects])]
})
export class LdapDomainModule {}
