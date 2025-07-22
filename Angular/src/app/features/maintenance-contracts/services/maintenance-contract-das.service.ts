import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'biang/core';
import { MaintenanceContract } from '../model/maintenance-contract';

@Injectable({
  providedIn: 'root',
})
export class MaintenanceContractDas extends AbstractDas<MaintenanceContract> {
  constructor(injector: Injector) {
    super(injector, 'MaintenanceContracts');
  }
}
