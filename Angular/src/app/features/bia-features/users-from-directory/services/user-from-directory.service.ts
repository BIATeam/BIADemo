import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserFromDirectory } from '../model/user-from-Directory';

@Injectable({
  providedIn: 'root'
})
export class UserFromDirectoryService {
  constructor() {}

  public formatDisplayNameFromADObs(user$: Observable<Array<UserFromDirectory>>): Observable<Array<UserFromDirectory>> {
    return user$.pipe(
      map((users: UserFromDirectory[]) => {
        return this.formatDisplayNamesFromAD(users);
      })
    );
  }

  public formatDisplayNamesFromAD(users: Array<UserFromDirectory>): Array<UserFromDirectory> {
    users.forEach((user: UserFromDirectory) => {
      user = this.formatDisplayNameFromAD(user);
    });

    return users;
  }

  public formatDisplayNameFromAD(user: UserFromDirectory): UserFromDirectory {
    user.displayName = `${user.lastName} ${user.firstName} (${user.domain}\\${user.login})`;
    return user;
  }
}
