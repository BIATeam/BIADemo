import { Component } from '@angular/core';
import { BiaFormComponent, CrudItemFormComponent } from 'bia-ng/shared';
import { MaintenanceContract } from '../../model/maintenance-contract';

@Component({
  selector: 'app-maintenance-contract-form',
  templateUrl:
    '../../../../../../node_modules/bia-ng/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../../../node_modules/bia-ng/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class MaintenanceContractFormComponent extends CrudItemFormComponent<MaintenanceContract> {}
