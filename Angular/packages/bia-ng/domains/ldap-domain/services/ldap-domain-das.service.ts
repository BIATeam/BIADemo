import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'bia-ng/core';
import { Observable } from 'rxjs';
import { LdapDomain } from '../model/ldap-domain';
import { LdapDomainService } from './ldap-domain.service';

@Injectable({
  providedIn: 'root',
})
export class LdapDomainDas extends AbstractDas<LdapDomain> {
  constructor(
    injector: Injector,
    protected ldapDomainService: LdapDomainService
  ) {
    super(injector, 'ldapDomains');
  }

  public getAll(): Observable<Array<LdapDomain>> {
    return this.ldapDomainService.formatDisplayNameFromObs(
      this.http.get<Array<LdapDomain>>(this.route)
    );
  }
}
