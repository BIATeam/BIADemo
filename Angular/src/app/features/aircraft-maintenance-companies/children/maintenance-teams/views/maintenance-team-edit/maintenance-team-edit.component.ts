import { Component, Injector } from '@angular/core';
import { MaintenanceTeam } from '../../model/maintenance-team';
import { MaintenanceTeamCRUDConfiguration } from '../../maintenance-team.constants';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { MaintenanceTeamService } from '../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-team-edit',
  templateUrl: './maintenance-team-edit.component.html',
})
export class MaintenanceTeamEditComponent extends CrudItemEditComponent<MaintenanceTeam> {
  constructor(
    protected injector: Injector,
    public maintenanceTeamService: MaintenanceTeamService,
  ) {
    super(injector, maintenanceTeamService);
    this.crudConfiguration = MaintenanceTeamCRUDConfiguration;
  }
}
