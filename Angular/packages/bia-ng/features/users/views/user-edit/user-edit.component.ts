import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import {
  CrudItemEditComponent,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';
import { UserFormComponent } from '../../components/user-form/user-form.component';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';
import { userCRUDConfiguration } from '../../user.constants';

@Component({
  selector: 'bia-user-edit',
  templateUrl: './user-edit.component.html',
  imports: [UserFormComponent, AsyncPipe, SpinnerComponent],
})
export class UserEditComponent extends CrudItemEditComponent<User> {
  constructor(
    protected injector: Injector,
    public userService: UserService
  ) {
    super(injector, userService);
    this.crudConfiguration = userCRUDConfiguration;
  }
}
