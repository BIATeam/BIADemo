import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { LdapDomainsEffects } from './store/ldap-domain-effects';
import { reducers } from './store/ldap-domain.state';

@NgModule({
  imports: [
    StoreModule.forFeature('domain-ldap-domains', reducers),
    EffectsModule.forFeature([LdapDomainsEffects]),
  ],
})
export class LdapDomainModule {}
