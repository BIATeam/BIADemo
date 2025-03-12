import { Component, Injector } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemReadComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-read/crud-item-read.component';
import { Permission } from 'src/app/shared/permission';
import { PlaneSpecific } from '../../model/plane-specific';
import { planeSpecificCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';
import { NgIf, AsyncPipe } from '@angular/common';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { BiaSharedModule } from '../../../../shared/bia-shared/bia-shared.module';

@Component({
    selector: 'app-plane-specific-read',
    templateUrl: './plane-read.component.html',
    imports: [NgIf, PlaneFormComponent, BiaSharedModule, AsyncPipe]
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
