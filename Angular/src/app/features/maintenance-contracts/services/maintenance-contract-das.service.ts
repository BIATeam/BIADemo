import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from '@bia-team/bia-ng/core';
import { MaintenanceContract } from '../model/maintenance-contract';

@Injectable({
  providedIn: 'root',
})
export class MaintenanceContractDas extends AbstractDas<MaintenanceContract> {
  constructor(injector: Injector) {
    super(injector, 'MaintenanceContracts');
  }
}
