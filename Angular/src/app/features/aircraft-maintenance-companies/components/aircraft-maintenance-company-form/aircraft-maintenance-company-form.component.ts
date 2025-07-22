import { Component } from '@angular/core';
import { BiaFormComponent, CrudItemFormComponent } from 'biang/shared';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';

@Component({
  selector: 'app-aircraft-maintenance-company-form',
  templateUrl:
    '../../../../../../node_modules/biang/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../../../node_modules/biang/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class AircraftMaintenanceCompanyFormComponent extends CrudItemFormComponent<AircraftMaintenanceCompany> {}
