import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from '@bia-team/bia-ng/core';
import { LdapDomain } from '@bia-team/bia-ng/models';
import { Observable } from 'rxjs';
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
