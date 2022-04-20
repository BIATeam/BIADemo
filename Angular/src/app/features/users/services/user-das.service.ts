import { Injectable, Injector } from '@angular/core';
import { User } from '../model/user';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';

@Injectable({
  providedIn: 'root'
})
export class UserDas extends AbstractDas<User> {
  constructor(injector: Injector) {
    super(injector, 'Users');
  }

  public synchronize() {
    return this.http.get(`${this.route}synchronize`);
  }
}
