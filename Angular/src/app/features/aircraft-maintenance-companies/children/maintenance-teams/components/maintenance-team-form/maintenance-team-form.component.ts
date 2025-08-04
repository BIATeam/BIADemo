import { Component } from '@angular/core';
import { BiaFormComponent, CrudItemFormComponent } from 'bia-ng/shared';
import { MaintenanceTeam } from '../../model/maintenance-team';

@Component({
  selector: 'app-maintenance-team-form',
  templateUrl:
    '../../../../../../../../node_modules/bia-ng/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../../../../../node_modules/bia-ng/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class MaintenanceTeamFormComponent extends CrudItemFormComponent<MaintenanceTeam> {}
