import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { User } from '../model/user';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class UserDas extends AbstractDas<User> {
  constructor(injector: Injector, private userService: UserService) {
    super(injector, 'users');
  }

  public getAllByFilter(filter: string): Observable<Array<User>> {
    return this.userService.formatDisplayNameObs(this.http.get<Array<User>>(`${this.route}?filter=${filter}`));
  }

  public synchronize() {
    return this.http.get(`${this.route}synchronize`);
  }
}
