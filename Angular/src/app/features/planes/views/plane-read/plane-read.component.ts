import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector } from '@angular/core';
import {
  CrudItemReadComponent,
  FormReadOnlyMode,
  SpinnerComponent,
} from 'biang/shared';
import { filter } from 'rxjs';
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
    public planeService: PlaneService
  ) {
    super(injector, planeService);
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
