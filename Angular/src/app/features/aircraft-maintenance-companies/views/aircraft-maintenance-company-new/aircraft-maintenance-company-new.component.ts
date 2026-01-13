import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { CrudItemNewComponent } from '@bia-team/bia-ng/shared';
import { aircraftMaintenanceCompanyCRUDConfiguration } from '../../aircraft-maintenance-company.constants';
import { AircraftMaintenanceCompanyFormComponent } from '../../components/aircraft-maintenance-company-form/aircraft-maintenance-company-form.component';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';
import { AircraftMaintenanceCompanyService } from '../../services/aircraft-maintenance-company.service';

@Component({
  selector: 'app-aircraft-maintenance-company-new',
  templateUrl: './aircraft-maintenance-company-new.component.html',
  imports: [AircraftMaintenanceCompanyFormComponent, AsyncPipe],
})
export class AircraftMaintenanceCompanyNewComponent
  extends CrudItemNewComponent<AircraftMaintenanceCompany>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService
  ) {
    super(injector, aircraftMaintenanceCompanyService);
    this.crudConfiguration = aircraftMaintenanceCompanyCRUDConfiguration;
  }
}
