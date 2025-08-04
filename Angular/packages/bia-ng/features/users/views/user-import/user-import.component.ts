import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { Permission } from 'packages/bia-ng/core/public-api';
import {
  BiaFormComponent,
  CrudItemImportComponent,
  CrudItemImportFormComponent,
} from 'packages/bia-ng/shared/public-api';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';
import { userCRUDConfiguration } from '../../user.constants';

@Component({
  selector: 'bia-user-import',
  templateUrl:
    '../../../../shared/feature-templates/crud-items/views/crud-item-import/crud-item-import.component.html',
  imports: [CrudItemImportFormComponent, AsyncPipe, BiaFormComponent],
})
export class UserImportComponent extends CrudItemImportComponent<User> {
  constructor(
    protected injector: Injector,
    protected userService: UserService
  ) {
    super(injector, userService);
    this.crudConfiguration = userCRUDConfiguration;
    this.setPermissions();
  }

  setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.User_UpdateRoles);
    this.canDelete = this.authService.hasPermission(Permission.User_Delete);
    this.canAdd = this.authService.hasPermission(Permission.User_Add);
  }

  save(toSaves: User[]): void {
    this.userService.save(toSaves);
  }
}
