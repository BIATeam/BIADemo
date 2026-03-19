import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from '@bia-team/bia-ng/core';
import { AircraftMaintenanceCompany } from '../model/aircraft-maintenance-company';

@Injectable({
  providedIn: 'root',
})
export class AircraftMaintenanceCompanyDas extends AbstractDas<AircraftMaintenanceCompany> {
  constructor(injector: Injector) {
    super(injector, 'AircraftMaintenanceCompanies');
  }
}
