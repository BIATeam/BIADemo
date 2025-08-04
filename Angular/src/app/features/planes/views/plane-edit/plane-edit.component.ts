import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import {
  CrudItemEditComponent,
  FormReadOnlyMode,
  SpinnerComponent,
} from 'bia-ng/shared';
import { filter } from 'rxjs';
import { Permission } from 'src/app/shared/permission';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { Plane } from '../../model/plane';
import { planeCRUDConfiguration } from '../../plane.constants';
import { PlaneOptionsService } from '../../services/plane-options.service';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-edit',
  templateUrl: './plane-edit.component.html',
  imports: [NgIf, PlaneFormComponent, AsyncPipe, SpinnerComponent],
})
export class PlaneEditComponent
  extends CrudItemEditComponent<Plane>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    protected planeOptionsService: PlaneOptionsService,
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
