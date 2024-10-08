import { Component, Injector, OnInit } from '@angular/core';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { maintenanceTeamCRUDConfiguration } from '../../maintenance-team.constants';
import { MaintenanceTeam } from '../../model/maintenance-team';
// BIAToolKit - Begin Option
import { MaintenanceTeamOptionsService } from '../../services/maintenance-team-options.service';
// BIAToolKit - End Option
import { MaintenanceTeamService } from '../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-team-new',
  templateUrl: './maintenance-team-new.component.html',
})
export class MaintenanceTeamNewComponent
  extends CrudItemNewComponent<MaintenanceTeam>
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
