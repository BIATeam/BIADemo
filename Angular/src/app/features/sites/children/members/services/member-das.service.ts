import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from '../model/member';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';

@Injectable({
  providedIn: 'root'
})
export class MemberDas extends AbstractDas<Member> {
  constructor(injector: Injector) {
    super(injector, 'members');
  }

  public getAllBySite(siteId: number): Observable<Array<Member>> {
    return this.http.get<Array<Member>>(`${AbstractDas.buildRoute('sites')}${siteId}/members`);
  }
}
