import { AsyncPipe, NgClass } from '@angular/common';
import { Component, Injector, ViewChild } from '@angular/core';
import { AuthService } from '@bia-team/bia-ng/core';
import {
  BiaTableBehaviorControllerComponent,
  BiaTableComponent,
  BiaTableControllerComponent,
  BiaTableHeaderComponent,
  CrudItemService,
  CrudItemsIndexComponent,
} from '@bia-team/bia-ng/shared';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
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
    PlaneTypeTableComponent,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
  ],
  providers: [{ provide: CrudItemService, useExisting: PlaneTypeService }],
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
