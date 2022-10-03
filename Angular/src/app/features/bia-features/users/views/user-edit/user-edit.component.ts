import { Component, Injector } from '@angular/core';
import { User } from '../../model/user';
import { UserCRUDConfiguration } from '../../user.constants';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'bia-user-edit',
  templateUrl: './user-edit.component.html',
})
export class UserEditComponent extends CrudItemEditComponent<User> {
  constructor(
    protected injector: Injector,
    public userService: UserService,
  ) {
    super(injector, userService);
    this.crudConfiguration = UserCRUDConfiguration;
  }
}
