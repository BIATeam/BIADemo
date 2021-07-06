import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { LdapDomain } from '../../ldap-domain/model/ldap-domain';

@Injectable({
  providedIn: 'root'
})
export class LdapDomainService {
  constructor() {}

  public formatDisplayNameFromObs(ldapDomains$: Observable<Array<LdapDomain>>): Observable<Array<LdapDomain>> {
    return ldapDomains$.pipe(
      map((ldapDomains: LdapDomain[]) => {
        return this.formatDisplayNames(ldapDomains);
      })
    );
  }

  public formatDisplayNames(ldapDomains: Array<LdapDomain>): Array<LdapDomain> {
    ldapDomains.forEach((domain: LdapDomain) => {
      domain = this.formatDisplayName(domain);
    });

    return ldapDomains;
  }

  public formatDisplayName(domain: LdapDomain): LdapDomain {
    domain.displayName = `${domain.name} (${domain.ldapName})`;
    return domain;
  }
}
