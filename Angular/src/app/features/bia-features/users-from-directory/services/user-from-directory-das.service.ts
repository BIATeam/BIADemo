import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { UserFromDirectory } from '../model/user-from-directory';

@Injectable({
  providedIn: 'root'
})
export class UserFromDirectoryDas extends AbstractDas<UserFromDirectory> {
  constructor(injector: Injector) {
    super(injector, 'users');
  }

  public getAllByFilter(filter: string, ldapName: string, returnSize: number): Observable<Array<UserFromDirectory>> {
    return this.http.get<Array<UserFromDirectory>>(`${this.route}fromAD?filter=${filter}&ldapName=${ldapName}&returnSize=${returnSize}`);
  }
}
