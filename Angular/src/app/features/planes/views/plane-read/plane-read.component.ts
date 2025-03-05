import { Component, Injector, OnInit } from '@angular/core';
import { filter } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { FormReadOnlyMode } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
import { CrudItemReadComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-read/crud-item-read.component';
import { Permission } from 'src/app/shared/permission';
import { Plane } from '../../model/plane';
import { planeCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-read',
  templateUrl: './plane-read.component.html',
})
export class PlaneReadComponent
  extends CrudItemReadComponent<Plane>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    public planeService: PlaneService,
    protected authService: AuthService
  ) {
    super(injector, planeService, authService);
    this.crudConfiguration = planeCRUDConfiguration;
  }

  ngOnInit(): void {
    super.ngOnInit();
  }

  protected setPermissions(): void {
    this.canFix = this.authService.hasPermission(Permission.Plane_Fix);
    console.log('canFix', this.canFix);
    this.sub.add(
      this.planeService.crudItem$
        .pipe(filter(plane => !!plane && Object.keys(plane).length > 0))
        .subscribe(plane => {
          this.canEdit =
            plane.isFixed === true
              ? this.canFix
              : this.authService.hasPermission(Permission.Plane_Update);

          this.formReadOnlyMode =
            plane.isFixed === true
              ? this.canEdit
                ? this.formReadOnlyMode
                : FormReadOnlyMode.on
              : this.formReadOnlyMode;
        })
    );
  }
}
