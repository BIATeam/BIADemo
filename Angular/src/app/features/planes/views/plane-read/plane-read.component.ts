import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { filter } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { FormReadOnlyMode } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
import { CrudItemReadComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-read/crud-item-read.component';
import { Permission } from 'src/app/shared/permission';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { Plane } from '../../model/plane';
import { planeCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-read',
  templateUrl: './plane-read.component.html',
  imports: [NgIf, PlaneFormComponent, AsyncPipe, SpinnerComponent],
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
}
