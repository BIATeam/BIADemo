import { Component, Injector } from '@angular/core';
import { MaintenanceTeam } from '../../model/maintenance-team';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { MaintenanceTeamService } from '../../services/maintenance-team.service';
import { MaintenanceTeamCRUDConfiguration } from '../../maintenance-team.constants';

@Component({
  selector: 'app-maintenance-team-new',
  templateUrl: './maintenance-team-new.component.html',
})
export class MaintenanceTeamNewComponent extends CrudItemNewComponent<MaintenanceTeam> {
  constructor(
    protected injector: Injector,
    public maintenanceTeamService: MaintenanceTeamService
  ) {
    super(injector, maintenanceTeamService);
    this.crudConfiguration = MaintenanceTeamCRUDConfiguration;
  }
}
