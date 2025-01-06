import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { AircraftMaintenanceCompany } from '../model/aircraft-maintenance-company';

@Injectable({
  providedIn: 'root',
})
export class AircraftMaintenanceCompanyDas extends AbstractDas<AircraftMaintenanceCompany> {
  constructor(injector: Injector) {
    super(injector, 'AircraftMaintenanceCompanies');
  }
}
