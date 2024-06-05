import { Component, Injector, ViewChild } from '@angular/core';
import { PlaneType } from '../../model/plane-type';
import { PlaneTypeCRUDConfiguration } from '../../plane-type.constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { PlaneTypeService } from '../../services/plane-type.service';
import { PlaneTypeTableComponent } from '../../components/plane-type-table/plane-type-table.component';

@Component({
  selector: 'app-planes-types-index',
  templateUrl: './planes-types-index.component.html',
  styleUrls: ['./planes-types-index.component.scss'],
})
export class PlanesTypesIndexComponent extends CrudItemsIndexComponent<PlaneType> {
  @ViewChild(PlaneTypeTableComponent, { static: false })
  crudItemTableComponent: PlaneTypeTableComponent;

  constructor(
    protected injector: Injector,
    public planeTypeService: PlaneTypeService,
    protected authService: AuthService
  ) {
    super(injector, planeTypeService);
    this.crudConfiguration = PlaneTypeCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.PlaneType_Update);
    this.canDelete = this.authService.hasPermission(
      Permission.PlaneType_Delete
    );
    this.canAdd = this.authService.hasPermission(Permission.PlaneType_Create);
  }
}
