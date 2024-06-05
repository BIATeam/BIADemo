import { Component, Injector } from '@angular/core';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';
import { AircraftMaintenanceCompanyCRUDConfiguration } from '../../aircraft-maintenance-company.constants';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { AircraftMaintenanceCompanyService } from '../../services/aircraft-maintenance-company.service';

@Component({
  selector: 'app-aircraft-maintenance-company-edit',
  templateUrl: './aircraft-maintenance-company-edit.component.html',
})
export class AircraftMaintenanceCompanyEditComponent extends CrudItemEditComponent<AircraftMaintenanceCompany> {
  constructor(
    protected injector: Injector,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService
  ) {
    super(injector, aircraftMaintenanceCompanyService);
    this.crudConfiguration = AircraftMaintenanceCompanyCRUDConfiguration;
  }
}
