import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'bia-ng/core';
import { User } from '../model/user';

@Injectable({
  providedIn: 'root',
})
export class UserDas extends AbstractDas<User> {
  constructor(injector: Injector) {
    super(injector, 'Users');
  }

  public synchronize() {
    return this.http.get(`${this.route}synchronize`);
  }
}
