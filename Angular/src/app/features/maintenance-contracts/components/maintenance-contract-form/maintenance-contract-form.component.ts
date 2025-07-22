import { Component } from '@angular/core';
import { BiaFormComponent, CrudItemFormComponent } from 'biang/shared';
import { MaintenanceContract } from '../../model/maintenance-contract';

@Component({
  selector: 'app-maintenance-contract-form',
  templateUrl:
    '../../../../../../node_modules/biang/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../../../node_modules/biang/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class MaintenanceContractFormComponent extends CrudItemFormComponent<MaintenanceContract> {}
