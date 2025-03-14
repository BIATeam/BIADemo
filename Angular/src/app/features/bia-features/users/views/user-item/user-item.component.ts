import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { Component, Injector, OnInit } from '@angular/core';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';
import { RouterOutlet } from '@angular/router';
import { NgIf, AsyncPipe } from '@angular/common';
import { BiaSharedModule } from '../../../../../shared/bia-shared/bia-shared.module';

@Component({
  selector: 'bia-users-item',
  templateUrl:
    '../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, NgIf, BiaSharedModule, AsyncPipe, SpinnerComponent],
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
