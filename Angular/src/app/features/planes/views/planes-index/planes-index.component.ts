import { Component, Injector, ViewChild } from '@angular/core';
import { Plane } from '../../model/plane';
import { planeCRUDConfiguration } from '../../plane.constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { PlaneService } from '../../services/plane.service';
import { PlaneTableComponent } from '../../components/plane-table/plane-table.component';

@Component({
  selector: 'app-planes-index',
  templateUrl: './planes-index.component.html',
  styleUrls: ['./planes-index.component.scss'],
})
export class PlanesIndexComponent extends CrudItemsIndexComponent<Plane> {
  @ViewChild(PlaneTableComponent, { static: false })
  crudItemTableComponent: PlaneTableComponent;
  /// BIAToolKit - Begin Child Engine
  canViewEngines = false;
  /// BIAToolKit - End Child Engine
  constructor(
    protected injector: Injector,
    public planeService: PlaneService,
    protected authService: AuthService
  ) {
    super(injector, planeService);
    this.crudConfiguration = planeCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
    this.canDelete = this.authService.hasPermission(Permission.Plane_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Plane_Create);
    /// BIAToolKit - Begin Child Engine
    this.canViewEngines = this.authService.hasPermission(
      Permission.Engine_List_Access
    );
    /// BIAToolKit - End Child Engine
  }

  /// BIAToolKit - Begin Child Engine
  onViewEngines(crudItemId: any) {
    if (crudItemId && crudItemId > 0) {
      this.router.navigate([crudItemId, 'engines'], {
        relativeTo: this.activatedRoute,
      });
    }
  }
  /// BIAToolKit - End Child Engine
}
