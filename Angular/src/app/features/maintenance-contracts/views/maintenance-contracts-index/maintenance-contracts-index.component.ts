import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component, Injector, ViewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTableBehaviorControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-behavior-controller/bia-table-behavior-controller.component';
import { BiaTableControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
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
    NgIf,
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
