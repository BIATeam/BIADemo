import { Component, Injector } from '@angular/core';
// Begin BIADemo
import { filter } from 'rxjs';
// End BIADemo
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
// Begin BIADemo
import { FormReadOnlyMode } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
// End BIADemo
import { CrudItemReadComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-read/crud-item-read.component';
// Begin BIADemo
import { Permission } from 'src/app/shared/permission';
// End BIADemo
import { Plane } from '../../model/plane';
import { planeCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';
import { NgIf, AsyncPipe } from '@angular/common';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { SpinnerComponent } from '../../../../shared/bia-shared/components/spinner/spinner.component';

@Component({
    selector: 'app-plane-read',
    templateUrl: './plane-read.component.html',
    imports: [NgIf, PlaneFormComponent, SpinnerComponent, AsyncPipe]
})
export class PlaneReadComponent extends CrudItemReadComponent<Plane> {
  constructor(
    protected injector: Injector,
    public planeService: PlaneService,
    protected authService: AuthService
  ) {
    super(injector, planeService, authService);
    this.crudConfiguration = planeCRUDConfiguration;
  }
  // Begin BIADemo
  protected setPermissions(): void {
    super.setPermissions();
    this.canFix = this.authService.hasPermission(Permission.Plane_Fix);
    this.permissionSub.add(
      this.crudItemService.crudItem$
        .pipe(filter(plane => !!plane && Object.keys(plane).length > 0))
        .subscribe(plane => {
          this.canEdit =
            this.crudConfiguration.isFixable === true && plane.isFixed === true
              ? false
              : this.authService.hasPermission(Permission.Plane_Update);

          this.formReadOnlyMode =
            this.canEdit === false &&
            this.crudConfiguration.isFixable === true &&
            plane.isFixed === true
              ? FormReadOnlyMode.on
              : this.initialFormReadOnlyMode;
        })
    );
  }
  // End BIADemo
}
