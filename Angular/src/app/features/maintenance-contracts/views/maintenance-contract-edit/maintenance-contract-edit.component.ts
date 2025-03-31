import { Component, Injector } from '@angular/core';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { MaintenanceContract } from '../../model/maintenance-contract';
import { maintenanceContractCRUDConfiguration } from '../../maintenance-contract.constants';
import { MaintenanceContractService } from '../../services/maintenance-contract.service';

@Component({
  selector: 'app-maintenance-contract-edit',
  templateUrl: './maintenance-contract-edit.component.html',
})
export class MaintenanceContractEditComponent extends CrudItemEditComponent<MaintenanceContract> {
  constructor(
    protected injector: Injector,
    public maintenanceContractService: MaintenanceContractService
  ) {
    super(injector, maintenanceContractService);
    this.crudConfiguration = maintenanceContractCRUDConfiguration;
  }
}
