import { Component, Injector, OnInit } from '@angular/core';
import { MaintenanceTeamService } from '../../../../services/maintenance-team.service';
import { MemberEditComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component';
import { TeamTypeId } from 'src/app/shared/constants';

@Component({
  selector: 'app-maintenance-team-member-edit',
  templateUrl:
    '../../../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.html',
})
export class MaintenanceTeamMemberEditComponent
  extends MemberEditComponent
  implements OnInit
{
  constructor(
    injector: Injector,
    public maintenanceTeamService: MaintenanceTeamService
  ) {
    super(injector);
  }

  ngOnInit() {
    this.teamTypeId = TeamTypeId.MaintenanceTeam;
    super.ngOnInit();
  }
}
