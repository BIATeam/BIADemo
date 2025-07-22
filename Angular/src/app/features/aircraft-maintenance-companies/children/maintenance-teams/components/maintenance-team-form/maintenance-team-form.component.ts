import { Component } from '@angular/core';
import { BiaFormComponent, CrudItemFormComponent } from 'biang/shared';
import { MaintenanceTeam } from '../../model/maintenance-team';

@Component({
  selector: 'app-maintenance-team-form',
  templateUrl:
    '../../../../../../../../node_modules/biang/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../../../../../node_modules/biang/templates/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class MaintenanceTeamFormComponent extends CrudItemFormComponent<MaintenanceTeam> {}
