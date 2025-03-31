import { Component } from '@angular/core';
import { BiaFormComponent } from 'src/app/shared/bia-shared/components/form/bia-form/bia-form.component';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { MaintenanceContract } from '../../model/maintenance-contract';

@Component({
  selector: 'app-maintenance-contract-form',
  templateUrl:
    '/src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '/src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class MaintenanceContractFormComponent extends CrudItemFormComponent<MaintenanceContract> {}
