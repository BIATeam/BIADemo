import { Component, Injector } from '@angular/core';
import { CrudItemImportComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-import/crud-item-import.component';
import { Permission } from 'src/app/shared/permission';
import { Plane } from '../../model/plane';
import { planeCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-import',
  templateUrl:
    '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-import/crud-item-import.component.html',
})
export class PlaneImportComponent extends CrudItemImportComponent<Plane> {
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
