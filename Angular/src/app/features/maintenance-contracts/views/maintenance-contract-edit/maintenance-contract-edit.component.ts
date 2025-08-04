import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector } from '@angular/core';
import {
  CrudItemEditComponent,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';
import { MaintenanceContractFormComponent } from '../../components/maintenance-contract-form/maintenance-contract-form.component';
import { maintenanceContractCRUDConfiguration } from '../../maintenance-contract.constants';
import { MaintenanceContract } from '../../model/maintenance-contract';
import { MaintenanceContractService } from '../../services/maintenance-contract.service';

@Component({
  selector: 'app-maintenance-contract-edit',
  templateUrl: './maintenance-contract-edit.component.html',
  imports: [
    NgIf,
    MaintenanceContractFormComponent,
    AsyncPipe,
    SpinnerComponent,
  ],
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
