import { Component, Injector } from '@angular/core';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';
import { userCRUDConfiguration } from '../../user.constants';

@Component({
  selector: 'bia-user-edit',
  templateUrl: './user-edit.component.html',
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
