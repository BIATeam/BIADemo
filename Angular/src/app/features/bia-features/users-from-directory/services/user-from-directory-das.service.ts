import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { UserFromDirectoryService } from './user-from-Directory.service';
import { UserFromDirectory } from '../model/user-from-Directory';

@Injectable({
  providedIn: 'root'
})
export class UserFromDirectoryDas extends AbstractDas<UserFromDirectory> {
  constructor(injector: Injector, private userService: UserFromDirectoryService) {
    super(injector, 'users');
  }

  public getAllByFilter(filter: string, ldapName: string, returnSize: number): Observable<Array<UserFromDirectory>> {
    return this.userService.formatDisplayNameFromADObs(
      this.http.get<Array<UserFromDirectory>>(`${this.route}fromAD?filter=${filter}&ldapName=${ldapName}&returnSize=${returnSize}`)
    );
  }
}
