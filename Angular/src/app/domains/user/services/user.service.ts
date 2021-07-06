import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../model/user';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor() {}

  public formatDisplayNameObs(user$: Observable<Array<User>>): Observable<Array<User>> {
    return user$.pipe(
      map((users: User[]) => {
        return this.formatDisplayNames(users);
      })
    );
  }

  public formatDisplayNames(users: Array<User>): Array<User> {
    users.forEach((user: User) => {
      user = this.formatDisplayName(user);
    });

    return users;
  }

  public formatDisplayName(user: User): User {
    user.displayName = `${user.firstName} ${user.lastName} (${user.login})`;
    return user;
  }

}
