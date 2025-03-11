import { Component, Injector, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { Permission } from 'src/app/shared/permission';
import { PlaneTableComponent } from '../../components/plane-table/plane-table.component';
import { Plane } from '../../model/plane';
import { planeCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';

@Component({
    selector: 'app-planes-index',
    templateUrl: './planes-index.component.html',
    styleUrls: ['./planes-index.component.scss'],
    standalone: false
})
export class PlanesIndexComponent extends CrudItemsIndexComponent<Plane> {
  @ViewChild(PlaneTableComponent, { static: false })
  crudItemTableComponent: PlaneTableComponent;

  /// BIAToolKit - Begin Partial PlaneIndexTsCanViewChildDeclaration Engine
  canViewEngines = false;
  /// BIAToolKit - End Partial PlaneIndexTsCanViewChildDeclaration Engine
  // BIAToolKit - Begin PlaneIndexTsCanViewChildDeclaration
  // BIAToolKit - End PlaneIndexTsCanViewChildDeclaration
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
    this.canSave = this.authService.hasPermission(Permission.Plane_Save);
    // Begin BIADemo
    this.canFix = this.authService.hasPermission(Permission.Plane_Fix);
    // End BIADemo
    this.canSelect = this.canDelete;
    /// BIAToolKit - Begin Partial PlaneIndexTsCanViewChildSet Engine
    this.canViewEngines = this.authService.hasPermission(
      Permission.Engine_List_Access
    );
    this.canSelect = this.canSelect || this.canViewEngines;
    /// BIAToolKit - End Partial PlaneIndexTsCanViewChildSet Engine
    // BIAToolKit - Begin PlaneIndexTsCanViewChildSet
    // BIAToolKit - End PlaneIndexTsCanViewChildSet
  }

  /// BIAToolKit - Begin Partial PlaneIndexTsOnViewChild Engine
  onViewEngines(crudItemId: any) {
    if (crudItemId && crudItemId > 0) {
      this.router.navigate([crudItemId, 'engines'], {
        relativeTo: this.activatedRoute,
      });
    }
  }
  /// BIAToolKit - End Partial PlaneIndexTsOnViewChild Engine
  // BIAToolKit - Begin PlaneIndexTsOnViewChild
  // BIAToolKit - End PlaneIndexTsOnViewChild
}
