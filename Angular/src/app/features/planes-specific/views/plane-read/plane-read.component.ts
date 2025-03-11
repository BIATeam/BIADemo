import { Component, Injector } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemReadComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-read/crud-item-read.component';
import { Permission } from 'src/app/shared/permission';
import { PlaneSpecific } from '../../model/plane-specific';
import { planeSpecificCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';

@Component({
    selector: 'app-plane-specific-read',
    templateUrl: './plane-read.component.html',
    standalone: false
})
export class PlaneReadComponent extends CrudItemReadComponent<PlaneSpecific> {
  constructor(
    protected injector: Injector,
    public planeService: PlaneService,
    protected authService: AuthService
  ) {
    super(injector, planeService, authService);
    this.crudConfiguration = planeSpecificCRUDConfiguration;
  }

  setPermissions(): void {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
  }
}
