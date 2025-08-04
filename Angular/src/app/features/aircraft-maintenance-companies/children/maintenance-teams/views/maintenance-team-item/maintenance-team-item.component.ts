import { AsyncPipe, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  CrudItemItemComponent,
  CrudItemService,
  SpinnerComponent,
} from 'bia-ng/shared';
import { MaintenanceTeam } from '../../model/maintenance-team';
import { MaintenanceTeamService } from '../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-teams-item',
  templateUrl:
    '../../../../../../../../node_modules/bia-ng/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../../../node_modules/bia-ng/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, NgIf, AsyncPipe, SpinnerComponent],
  providers: [
    {
      provide: CrudItemService,
      useExisting: MaintenanceTeamService,
    },
  ],
})
export class MaintenanceTeamItemComponent extends CrudItemItemComponent<MaintenanceTeam> {}
