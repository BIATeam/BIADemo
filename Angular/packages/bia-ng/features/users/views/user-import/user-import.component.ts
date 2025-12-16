import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { BiaPermission } from '@bia-team/bia-ng/core';
import {
  BiaFormComponent,
  CrudItemImportComponent,
  CrudItemImportFormComponent,
} from '@bia-team/bia-ng/shared';
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
    this.canEdit = this.authService.hasPermission(
      BiaPermission.User_UpdateRoles
    );
    this.canDelete = this.authService.hasPermission(BiaPermission.User_Delete);
    this.canAdd = this.authService.hasPermission(BiaPermission.User_Add);
  }

  save(toSaves: User[]): void {
    this.userService.save(toSaves);
  }
}
