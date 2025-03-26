import { Component, Injector, OnInit } from '@angular/core';
import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { maintenanceTeamCRUDConfiguration } from '../../maintenance-team.constants';
import { MaintenanceTeam } from '../../model/maintenance-team';
// BIAToolKit - Begin Option
import { MaintenanceTeamOptionsService } from '../../services/maintenance-team-options.service';
// BIAToolKit - End Option
import { AsyncPipe, NgIf } from '@angular/common';
import { MaintenanceTeamFormComponent } from '../../components/maintenance-team-form/maintenance-team-form.component';
import { MaintenanceTeamService } from '../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-team-edit',
  templateUrl: './maintenance-team-edit.component.html',
  imports: [NgIf, MaintenanceTeamFormComponent, AsyncPipe, SpinnerComponent],
})
export class MaintenanceTeamEditComponent
  extends CrudItemEditComponent<MaintenanceTeam>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    // BIAToolKit - Begin Option
    protected maintenanceTeamOptionsService: MaintenanceTeamOptionsService,
    // BIAToolKit - End Option
    public maintenanceTeamService: MaintenanceTeamService
  ) {
    super(injector, maintenanceTeamService);
    this.crudConfiguration = maintenanceTeamCRUDConfiguration;
  }

  ngOnInit(): void {
    super.ngOnInit();
    // BIAToolKit - Begin Option
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.maintenanceTeamOptionsService.loadAllOptions();
      })
    );
    // BIAToolKit - End Option
  }
}
