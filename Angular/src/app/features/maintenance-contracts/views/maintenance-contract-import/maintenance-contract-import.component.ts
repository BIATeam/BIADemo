import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { BiaFormComponent } from 'src/app/shared/bia-shared/components/form/bia-form/bia-form.component';
import { CrudItemImportFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-import-form/crud-item-import-form.component';
import { CrudItemImportComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-import/crud-item-import.component';
import { Permission } from 'src/app/shared/permission';
import { maintenanceContractCRUDConfiguration } from '../../maintenance-contract.constants';
import { MaintenanceContract } from '../../model/maintenance-contract';
import { MaintenanceContractService } from '../../services/maintenance-contract.service';

@Component({
  selector: 'app-maintenance-contract-import',
  templateUrl:
    '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-import/crud-item-import.component.html',
  imports: [CrudItemImportFormComponent, AsyncPipe, BiaFormComponent],
})
export class MaintenanceContractImportComponent extends CrudItemImportComponent<MaintenanceContract> {
  constructor(
    protected injector: Injector,
    private maintenanceContractService: MaintenanceContractService
  ) {
    super(injector, maintenanceContractService);
    this.crudConfiguration = maintenanceContractCRUDConfiguration;
    this.setPermissions();
  }

  setPermissions() {
    this.canEdit = this.authService.hasPermission(
      Permission.MaintenanceContract_Update
    );
    this.canDelete = this.authService.hasPermission(
      Permission.MaintenanceContract_Delete
    );
    this.canAdd = this.authService.hasPermission(
      Permission.MaintenanceContract_Create
    );
  }

  save(toSaves: MaintenanceContract[]): void {
    this.maintenanceContractService.save(toSaves);
  }
}
