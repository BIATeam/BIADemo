import { Component, Injector, ViewChild } from '@angular/core';
import { MaintenanceTeam } from '../../model/maintenance-team';
import { MaintenanceTeamCRUDConfiguration } from '../../maintenance-team.constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { MaintenanceTeamService } from '../../services/maintenance-team.service';
import { MaintenanceTeamTableComponent } from '../../components/maintenance-team-table/maintenance-team-table.component';

@Component({
  selector: 'app-maintenance-teams-index',
  templateUrl: './maintenance-teams-index.component.html',
  styleUrls: ['./maintenance-teams-index.component.scss']
})

export class MaintenanceTeamsIndexComponent extends CrudItemsIndexComponent<MaintenanceTeam> {
  // Custo for teams
  canViewMembers = false;
  canSelectElement = false;

  @ViewChild(MaintenanceTeamTableComponent, { static: false }) crudItemTableComponent: MaintenanceTeamTableComponent;

  constructor(
    protected injector: Injector,
    public maintenanceTeamService: MaintenanceTeamService,
    protected authService: AuthService,
  ) {
    super(injector, maintenanceTeamService);
    this.crudConfiguration = MaintenanceTeamCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.MaintenanceTeam_Update);
    this.canDelete = this.authService.hasPermission(Permission.MaintenanceTeam_Delete);
    this.canAdd = this.authService.hasPermission(Permission.MaintenanceTeam_Create);
    // Custo for teams
    this.canViewMembers = this.authService.hasPermission(Permission.MaintenanceTeam_Member_List_Access);
    this.canSelectElement = 
      this.canDelete;
  }

  onClickRowData(crudItem: MaintenanceTeam) {
    if (crudItem.canMemberListAccess) {
      this.onViewMembers(crudItem.id);
    }
  }
  
  onViewMembers(crudItemId: any) {
    if (crudItemId && crudItemId > 0) {
      this.router.navigate([crudItemId, 'members'], { relativeTo: this.activatedRoute });
    }
  }
}
