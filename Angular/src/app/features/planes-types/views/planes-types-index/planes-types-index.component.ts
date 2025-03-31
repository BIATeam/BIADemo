import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component, Injector, ViewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTableBehaviorControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-behavior-controller/bia-table-behavior-controller.component';
import { BiaTableControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { Permission } from 'src/app/shared/permission';
import { PlaneTypeTableComponent } from '../../components/plane-type-table/plane-type-table.component';
import { PlaneType } from '../../model/plane-type';
import { planeTypeCRUDConfiguration } from '../../plane-type.constants';
import { PlaneTypeService } from '../../services/plane-type.service';

@Component({
  selector: 'app-planes-types-index',
  templateUrl: './planes-types-index.component.html',
  styleUrls: ['./planes-types-index.component.scss'],
  imports: [
    NgClass,

    PrimeTemplate,
    NgIf,
    PlaneTypeTableComponent,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
  ],
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
    this.crudConfiguration = planeTypeCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.PlaneType_Update);
    this.canDelete = this.authService.hasPermission(
      Permission.PlaneType_Delete
    );
    this.canAdd = this.authService.hasPermission(Permission.PlaneType_Create);
  }
}
