import { Component, Injector } from '@angular/core';
import { Plane } from '../../model/plane';
import { PlaneService } from '../../services/plane.service';
import { planeCRUDConfiguration } from '../../plane.constants';
import { CrudItemBulkComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-bulk/crud-item-bulk.component';
import { Permission } from 'src/app/shared/permission';

@Component({
  selector: 'app-plane-bulk',
  templateUrl:
    '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-bulk/crud-item-bulk.component.html',
})
export class PlaneBulkComponent extends CrudItemBulkComponent<Plane> {
  constructor(
    protected injector: Injector,
    private planeService: PlaneService
  ) {
    super(injector, planeService);
    this.crudConfiguration = planeCRUDConfiguration;
    this.setPermissions();
  }

  setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
    this.canDelete = this.authService.hasPermission(Permission.Plane_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Plane_Create);
  }

  save(toSaves: Plane[]): void {
    this.planeService.save(toSaves);
  }
}
