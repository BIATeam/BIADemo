import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { LdapDomain } from '../../ldap-domain/model/ldap-domain';

@Injectable({
  providedIn: 'root',
})
export class LdapDomainService {
  formatDisplayNameFromObs(
    ldapDomains$: Observable<Array<LdapDomain>>
  ): Observable<Array<LdapDomain>> {
    return ldapDomains$.pipe(
      map((ldapDomains: LdapDomain[]) => {
        return this.formatDisplayNames(ldapDomains);
      })
    );
  }

  formatDisplayNames(ldapDomains: Array<LdapDomain>): Array<LdapDomain> {
    return ldapDomains.map((domain: LdapDomain) => {
      return this.formatDisplayName(domain);
    });
  }

  formatDisplayName(domain: LdapDomain): LdapDomain {
    domain.displayName = `${domain.name} (${domain.ldapName})`;
    return domain;
  }
}
