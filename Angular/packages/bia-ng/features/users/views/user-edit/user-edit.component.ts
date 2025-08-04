import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { CrudItemEditComponent, SpinnerComponent } from 'bia-ng/shared';
import { UserFormComponent } from '../../components/user-form/user-form.component';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';
import { userCRUDConfiguration } from '../../user.constants';

@Component({
  selector: 'bia-user-edit',
  templateUrl: './user-edit.component.html',
  imports: [NgIf, UserFormComponent, AsyncPipe, SpinnerComponent],
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
