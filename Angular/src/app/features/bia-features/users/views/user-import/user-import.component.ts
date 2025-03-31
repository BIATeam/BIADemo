import { Component, Injector } from '@angular/core';
import { CrudItemImportComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-import/crud-item-import.component';
import { Permission } from 'src/app/shared/permission';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';
import { userCRUDConfiguration } from '../../user.constants';

@Component({
  selector: 'bia-user-import',
  templateUrl:
    '../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-import/crud-item-import.component.html',
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
