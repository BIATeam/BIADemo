import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { LdapDomain } from '../../ldap-domain/model/ldap-domain';

@Injectable({
  providedIn: 'root',
})
export class LdapDomainService {
  public formatDisplayNameFromObs(
    ldapDomains$: Observable<Array<LdapDomain>>
  ): Observable<Array<LdapDomain>> {
    return ldapDomains$.pipe(
      map((ldapDomains: LdapDomain[]) => {
        return this.formatDisplayNames(ldapDomains);
      })
    );
  }

  public formatDisplayNames(ldapDomains: Array<LdapDomain>): Array<LdapDomain> {
    return ldapDomains.map((domain: LdapDomain) => {
      return this.formatDisplayName(domain);
    });
  }

  public formatDisplayName(domain: LdapDomain): LdapDomain {
    domain.displayName = `${domain.name} (${domain.ldapName})`;
    return domain;
  }
}
