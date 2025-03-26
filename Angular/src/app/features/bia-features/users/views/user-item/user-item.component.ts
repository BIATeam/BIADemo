import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'bia-users-item',
  templateUrl:
    '../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, NgIf, AsyncPipe, SpinnerComponent],
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
