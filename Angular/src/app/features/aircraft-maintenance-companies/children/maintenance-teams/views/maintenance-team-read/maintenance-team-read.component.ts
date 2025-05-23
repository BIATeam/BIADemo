import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { CrudItemReadComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-read/crud-item-read.component';
import { MaintenanceTeamFormComponent } from '../../components/maintenance-team-form/maintenance-team-form.component';
import { maintenanceTeamCRUDConfiguration } from '../../maintenance-team.constants';
import { MaintenanceTeam } from '../../model/maintenance-team';
import { MaintenanceTeamOptionsService } from '../../services/maintenance-team-options.service';
import { MaintenanceTeamService } from '../../services/maintenance-team.service';
import { Permission } from 'src/app/shared/permission';
import { filter } from 'rxjs';
import { FormReadOnlyMode } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';

@Component({
  selector: 'app-maintenance-team-read',
  templateUrl: './maintenance-team-read.component.html',
  imports: [NgIf, MaintenanceTeamFormComponent, AsyncPipe, SpinnerComponent],
})
export class MaintenanceTeamReadComponent
  extends CrudItemReadComponent<MaintenanceTeam>
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

  protected setPermissions(): void {
    // Always call this to unsubscribe existing permission subscription
    super.setPermissions();

    // Define if user can fix
    this.canFix = this.authService.hasPermission(
      Permission.MaintenanceTeam_Fix
    );

    // Add the subscription to dedicated permission subscription
    this.permissionSub.add(
      this.crudItemService.crudItem$
        .pipe(filter(feature => !!feature && Object.keys(feature).length > 0))
        .subscribe(feature => {
          this.canEdit =
            this.crudConfiguration.isFixable === true &&
            feature.isFixed === true
              ? false
              : this.authService.hasPermission(Permission.Plane_Update);

          this.formReadOnlyMode =
            this.canEdit === false &&
            this.crudConfiguration.isFixable === true &&
            feature.isFixed === true
              ? FormReadOnlyMode.on
              : this.initialFormReadOnlyMode;
        })
    );
  }
}
