import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from '@bia-team/bia-ng/core';
import { Observable } from 'rxjs';
import { UserFromDirectory } from '../model/user-from-directory';

@Injectable({
  providedIn: 'root',
})
export class UserFromDirectoryDas extends AbstractDas<UserFromDirectory> {
  constructor(injector: Injector) {
    super(injector, 'users');
  }

  public getAllByFilter(
    filter: string,
    ldapName: string,
    returnSize: number
  ): Observable<Array<UserFromDirectory>> {
    if (ldapName === undefined) {
      return this.http.get<Array<UserFromDirectory>>(
        `${this.route}fromAD?filter=${filter}&returnSize=${returnSize}`
      );
    }
    return this.http.get<Array<UserFromDirectory>>(
      `${this.route}fromAD?filter=${filter}&ldapName=${ldapName}&returnSize=${returnSize}`
    );
  }
}
