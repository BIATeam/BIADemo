import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { UserFormComponent } from '../../components/user-form/user-form.component';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';
import { userCRUDConfiguration } from '../../user.constants';

@Component({
  selector: 'bia-user-new',
  templateUrl: './user-new.component.html',
  imports: [UserFormComponent, AsyncPipe],
})
export class UserNewComponent extends CrudItemNewComponent<User> {
  constructor(
    protected injector: Injector,
    public userService: UserService
  ) {
    super(injector, userService);
    this.crudConfiguration = userCRUDConfiguration;
  }
}
