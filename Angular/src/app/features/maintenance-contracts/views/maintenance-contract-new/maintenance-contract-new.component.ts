import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { CrudItemNewComponent } from 'packages/bia-ng/shared/public-api';
import { MaintenanceContractFormComponent } from '../../components/maintenance-contract-form/maintenance-contract-form.component';
import { maintenanceContractCRUDConfiguration } from '../../maintenance-contract.constants';
import { MaintenanceContract } from '../../model/maintenance-contract';
import { MaintenanceContractService } from '../../services/maintenance-contract.service';

@Component({
  selector: 'app-maintenance-contract-new',
  templateUrl: './maintenance-contract-new.component.html',
  imports: [MaintenanceContractFormComponent, AsyncPipe],
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
