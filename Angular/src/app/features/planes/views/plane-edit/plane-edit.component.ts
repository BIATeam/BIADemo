import { Component, Injector } from '@angular/core';
import { filter } from 'rxjs';
import { FormReadOnlyMode } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { Permission } from 'src/app/shared/permission';
import { Plane } from '../../model/plane';
import { planeCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-edit',
  templateUrl: './plane-edit.component.html',
})
export class PlaneEditComponent extends CrudItemEditComponent<Plane> {
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
          this.formReadOnlyMode =
            this.crudConfiguration.isFixable === true && plane.isFixed === true
              ? FormReadOnlyMode.on
              : FormReadOnlyMode.off;
        })
    );
  }
}
