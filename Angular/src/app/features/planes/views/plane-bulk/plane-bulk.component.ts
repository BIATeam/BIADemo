import { Component, Injector, ViewChild } from '@angular/core';
import { Plane } from '../../model/plane';
import { PlaneService } from '../../services/plane.service';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { PlaneCRUDConfiguration } from '../../plane.constants';
import { CrudItemBulkComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-bulk/crud-item-bulk.component';
import { Permission } from 'src/app/shared/permission';

@Component({
  selector: 'app-plane-bulk',
  templateUrl: './plane-bulk.component.html',
})
export class PlaneBulkComponent extends CrudItemBulkComponent<Plane> {
  @ViewChild(PlaneFormComponent) planeFormComponent: PlaneFormComponent;

  constructor(
    protected injector: Injector,
    public planeService: PlaneService
  ) {
    super(injector, planeService);
    this.crudConfiguration = PlaneCRUDConfiguration;
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

  getForm(): PlaneFormComponent {
    return this.planeFormComponent;
  }
}
