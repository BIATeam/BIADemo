import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { MaintenanceTeamFormComponent } from '../../components/maintenance-team-form/maintenance-team-form.component';
import { maintenanceTeamCRUDConfiguration } from '../../maintenance-team.constants';
import { MaintenanceTeam } from '../../model/maintenance-team';
import { MaintenanceTeamOptionsService } from '../../services/maintenance-team-options.service';
import { MaintenanceTeamService } from '../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-team-new',
  templateUrl: './maintenance-team-new.component.html',
  imports: [MaintenanceTeamFormComponent, AsyncPipe],
})
export class MaintenanceTeamNewComponent
  extends CrudItemNewComponent<MaintenanceTeam>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    protected maintenanceTeamOptionsService: MaintenanceTeamOptionsService,
    public maintenanceTeamService: MaintenanceTeamService
  ) {
    super(injector, maintenanceTeamService);
    this.crudConfiguration = maintenanceTeamCRUDConfiguration;
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.maintenanceTeamOptionsService.loadAllOptions();
      })
    );
  }
}
