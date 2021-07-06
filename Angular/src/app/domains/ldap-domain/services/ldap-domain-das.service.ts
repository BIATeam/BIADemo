import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { LdapDomain } from '../model/ldap-domain';
import { LdapDomainService } from './ldap-domain.service';

@Injectable({
  providedIn: 'root'
})
export class LdapDomainDas extends AbstractDas<LdapDomain> {
  constructor(injector: Injector, private ldapDomainService: LdapDomainService) {
    super(injector, 'ldapDomains');
  }

  public getAll(): Observable<Array<LdapDomain>> {
    return this.ldapDomainService.formatDisplayNameFromObs(this.http.get<Array<LdapDomain>>(this.route));
  }
}
