import { Component, Injector } from '@angular/core';
import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
// Begin BIADemo
import { filter } from 'rxjs';
import { FormReadOnlyMode } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
// End BIADemo
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
// Begin BIADemo
import { Permission } from 'src/app/shared/permission';
// End BIADemo
import { AsyncPipe, NgIf } from '@angular/common';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { Plane } from '../../model/plane';
import { planeCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-edit',
  templateUrl: './plane-edit.component.html',
  imports: [NgIf, PlaneFormComponent, AsyncPipe, SpinnerComponent],
})
export class PlaneEditComponent extends CrudItemEditComponent<Plane> {
  constructor(
    protected injector: Injector,
    public planeService: PlaneService
  ) {
    super(injector, planeService);
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
          this.formReadOnlyMode =
            this.crudConfiguration.isFixable === true && plane.isFixed === true
              ? FormReadOnlyMode.on
              : FormReadOnlyMode.off;
        })
    );
  }
  // End BIADemo
}
