import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { CrudItemReadComponent, SpinnerComponent } from 'biang/shared';
import { Permission } from 'src/app/shared/permission';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { PlaneSpecific } from '../../model/plane-specific';
import { planeSpecificCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-specific-read',
  templateUrl: './plane-read.component.html',
  imports: [NgIf, PlaneFormComponent, AsyncPipe, SpinnerComponent],
})
export class PlaneReadComponent extends CrudItemReadComponent<PlaneSpecific> {
  constructor(
    protected injector: Injector,
    public planeService: PlaneService
  ) {
    super(injector, planeService);
    this.crudConfiguration = planeSpecificCRUDConfiguration;
  }

  setPermissions(): void {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
  }
}
