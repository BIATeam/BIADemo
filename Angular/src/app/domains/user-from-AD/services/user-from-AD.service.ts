import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserFromAD } from '../model/user-from-AD';

@Injectable({
  providedIn: 'root'
})
export class UserFromADService {
  constructor() {}

  public formatDisplayNameFromADObs(user$: Observable<Array<UserFromAD>>): Observable<Array<UserFromAD>> {
    return user$.pipe(
      map((users: UserFromAD[]) => {
        return this.formatDisplayNamesFromAD(users);
      })
    );
  }

  public formatDisplayNamesFromAD(users: Array<UserFromAD>): Array<UserFromAD> {
    users.forEach((user: UserFromAD) => {
      user = this.formatDisplayNameFromAD(user);
    });

    return users;
  }

  public formatDisplayNameFromAD(user: UserFromAD): UserFromAD {
    user.displayName = `${user.firstName} ${user.lastName} (${user.domain}\\${user.login})`;
    return user;
  }
}
