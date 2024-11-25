import { Component, Injector, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { Permission } from 'src/app/shared/permission';
import { PlaneTableComponent } from '../../components/plane-table/plane-table.component';
import { Plane } from '../../model/plane';
import { planeSpecificCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-planes-specific-index',
  templateUrl: './planes-index.component.html',
  styleUrls: ['./planes-index.component.scss'],
})
export class PlanesIndexComponent extends CrudItemsIndexComponent<Plane> {
  @ViewChild(PlaneTableComponent, { static: false })
  crudItemTableComponent: PlaneTableComponent;

  constructor(
    protected injector: Injector,
    public planeService: PlaneService,
    protected authService: AuthService
  ) {
    super(injector, planeService);
    this.crudConfiguration = planeSpecificCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
    this.canDelete = this.authService.hasPermission(Permission.Plane_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Plane_Create);
  }
}
