import { Component, Injector } from '@angular/core';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { aircraftMaintenanceCompanyCRUDConfiguration } from '../../aircraft-maintenance-company.constants';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';
import { AircraftMaintenanceCompanyService } from '../../services/aircraft-maintenance-company.service';
import { NgIf, AsyncPipe } from '@angular/common';
import { AircraftMaintenanceCompanyFormComponent } from '../../components/aircraft-maintenance-company-form/aircraft-maintenance-company-form.component';
import { SpinnerComponent } from '../../../../shared/bia-shared/components/spinner/spinner.component';

@Component({
    selector: 'app-aircraft-maintenance-company-edit',
    templateUrl: './aircraft-maintenance-company-edit.component.html',
    imports: [NgIf, AircraftMaintenanceCompanyFormComponent, SpinnerComponent, AsyncPipe]
})
export class AircraftMaintenanceCompanyEditComponent extends CrudItemEditComponent<AircraftMaintenanceCompany> {
  constructor(
    protected injector: Injector,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService
  ) {
    super(injector, aircraftMaintenanceCompanyService);
    this.crudConfiguration = aircraftMaintenanceCompanyCRUDConfiguration;
  }
}
