import { Component, Injector, OnInit } from '@angular/core';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'bia-users-item',
  templateUrl:
    '/src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '/src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
})
export class UserItemComponent
  extends CrudItemItemComponent<User>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    public userService: UserService
  ) {
    super(injector, userService);
  }
}
