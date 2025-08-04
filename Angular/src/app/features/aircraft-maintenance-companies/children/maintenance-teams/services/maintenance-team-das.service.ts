import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'bia-ng/core';
import { MaintenanceTeam } from '../model/maintenance-team';

@Injectable({
  providedIn: 'root',
})
export class MaintenanceTeamDas extends AbstractDas<MaintenanceTeam> {
  constructor(injector: Injector) {
    super(injector, 'MaintenanceTeams');
  }
}
