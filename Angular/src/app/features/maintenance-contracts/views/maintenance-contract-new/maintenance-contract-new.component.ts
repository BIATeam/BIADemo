import { Component, Injector } from '@angular/core';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { MaintenanceContract } from '../../model/maintenance-contract';
import { maintenanceContractCRUDConfiguration } from '../../maintenance-contract.constants';
import { MaintenanceContractService } from '../../services/maintenance-contract.service';

@Component({
  selector: 'app-maintenance-contract-new',
  templateUrl: './maintenance-contract-new.component.html',
})
export class MaintenanceContractNewComponent extends CrudItemNewComponent<MaintenanceContract> {
  constructor(
    protected injector: Injector,
    public maintenanceContractService: MaintenanceContractService
  ) {
    super(injector, maintenanceContractService);
    this.crudConfiguration = maintenanceContractCRUDConfiguration;
  }
}
