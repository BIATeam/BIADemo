import { Injectable, Injector } from '@angular/core';
import { AircraftMaintenanceCompany } from '../model/aircraft-maintenance-company';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';

@Injectable({
  providedIn: 'root'
})
export class AircraftMaintenanceCompanyDas extends AbstractDas<AircraftMaintenanceCompany> {
  constructor(injector: Injector) {
    super(injector, 'AircraftMaintenanceCompanies');
  }
}
