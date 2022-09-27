import { Component, Injector, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { MembersIndexComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/members-index/members-index.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { Permission } from 'src/app/shared/permission';
import { MaintenanceTeamService } from '../../../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-team-members-index',
  templateUrl: '../../../../../../../../shared/bia-shared/feature-templates/members/views/members-index/members-index.component.html',
  styleUrls: ['../../../../../../../../shared/bia-shared/feature-templates/members/views/members-index/members-index.component.scss']
})
export class MaintenanceTeamMembersIndexComponent extends MembersIndexComponent implements OnInit {
  constructor(
    injector: Injector,
    public maintenanceTeamService: MaintenanceTeamService,
    private authService: AuthService
  ) {
    super(injector);
  }

  ngOnInit() {
    this.teamTypeId=TeamTypeId.MaintenanceTeam;
    super.ngOnInit();
    this.parentIds = [this.maintenanceTeamService.currentCrudItemId?.toString()];
  }
  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.MaintenanceTeam_Member_Update);
    this.canDelete = this.authService.hasPermission(Permission.MaintenanceTeam_Member_Delete);
    this.canAdd = this.authService.hasPermission(Permission.MaintenanceTeam_Member_Create);
  }
}
