import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserFromDirectory } from '../model/user-from-AD';

@Injectable({
  providedIn: 'root'
})
export class UserFromADService {
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
    user.displayName = `${user.firstName} ${user.lastName} (${user.domain}\\${user.login})`;
    return user;
  }
}
