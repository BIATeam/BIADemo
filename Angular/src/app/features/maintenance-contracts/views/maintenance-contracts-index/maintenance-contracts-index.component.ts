import { AsyncPipe, NgClass } from '@angular/common';
import { Component, Injector, ViewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from 'packages/bia-ng/core/public-api';
import {
  BiaTableBehaviorControllerComponent,
  BiaTableComponent,
  BiaTableControllerComponent,
  BiaTableHeaderComponent,
  CrudItemService,
  CrudItemsIndexComponent,
} from 'packages/bia-ng/shared/public-api';
import { PrimeTemplate } from 'primeng/api';
import { Permission } from 'src/app/shared/permission';
import { MaintenanceContractTableComponent } from '../../components/maintenance-contract-table/maintenance-contract-table.component';
import { maintenanceContractCRUDConfiguration } from '../../maintenance-contract.constants';
import { MaintenanceContract } from '../../model/maintenance-contract';
import { MaintenanceContractService } from '../../services/maintenance-contract.service';

@Component({
  selector: 'app-maintenance-contracts-index',
  templateUrl: './maintenance-contracts-index.component.html',
  styleUrls: ['./maintenance-contracts-index.component.scss'],
  imports: [
    NgClass,
    PrimeTemplate,
    MaintenanceContractTableComponent,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
  ],
  providers: [
    {
      provide: CrudItemService,
      useExisting: MaintenanceContractService,
    },
  ],
})
export class MaintenanceContractsIndexComponent extends CrudItemsIndexComponent<MaintenanceContract> {
  @ViewChild(MaintenanceContractTableComponent, { static: false })
  crudItemTableComponent: MaintenanceContractTableComponent;
  // BIAToolKit - Begin MaintenanceContractIndexTsCanViewChildDeclaration
  // BIAToolKit - End MaintenanceContractIndexTsCanViewChildDeclaration
  constructor(
    protected injector: Injector,
    public maintenanceContractService: MaintenanceContractService,
    protected authService: AuthService
  ) {
    super(injector, maintenanceContractService);
    this.crudConfiguration = maintenanceContractCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(
      Permission.MaintenanceContract_Update
    );
    this.canDelete = this.authService.hasPermission(
      Permission.MaintenanceContract_Delete
    );
    this.canAdd = this.authService.hasPermission(
      Permission.MaintenanceContract_Create
    );
    this.canSave = this.authService.hasPermission(
      Permission.MaintenanceContract_Save
    );
    this.canSelect = this.canDelete;
    // BIAToolKit - Begin MaintenanceContractIndexTsCanViewChildSet
    // BIAToolKit - End MaintenanceContractIndexTsCanViewChildSet
  }

  // BIAToolKit - Begin MaintenanceContractIndexTsOnViewChild
  // BIAToolKit - End MaintenanceContractIndexTsOnViewChild
}
