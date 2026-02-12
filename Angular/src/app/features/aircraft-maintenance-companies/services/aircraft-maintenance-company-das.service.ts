import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import {
  AircraftMaintenanceCompany,
  aircraftMaintenanceCompanyFieldsConfiguration,
} from '../model/aircraft-maintenance-company';

@Injectable({
  providedIn: 'root',
})
export class AircraftMaintenanceCompanyDas extends AbstractDas<AircraftMaintenanceCompany> {
  constructor(injector: Injector) {
    super(
      injector,
      'AircraftMaintenanceCompanies',
      aircraftMaintenanceCompanyFieldsConfiguration
    );
  }
}
