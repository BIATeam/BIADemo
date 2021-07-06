import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { UserFromADService } from './user-from-AD.service';
import { UserFromAD } from '../model/user-from-AD';

@Injectable({
  providedIn: 'root'
})
export class UserFromADDas extends AbstractDas<UserFromAD> {
  constructor(injector: Injector, private userService: UserFromADService) {
    super(injector, 'users');
  }

  public getAllByFilter(filter: string, ldapName: string): Observable<Array<UserFromAD>> {
    return this.userService.formatDisplayNameFromADObs(
      this.http.get<Array<UserFromAD>>(`${this.route}fromAD?filter=${filter}&ldapName=${ldapName}`)
    );
  }
}
