import { Component } from '@angular/core';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { MaintenanceTeam } from '../../model/maintenance-team';
import { BiaFormComponent } from '../../../../../../shared/bia-shared/components/form/bia-form/bia-form.component';

@Component({
    selector: 'app-maintenance-team-form',
    templateUrl: '../../../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
    styleUrls: [
        '../../../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
    ],
    imports: [BiaFormComponent]
})
export class MaintenanceTeamFormComponent extends CrudItemFormComponent<MaintenanceTeam> {}
