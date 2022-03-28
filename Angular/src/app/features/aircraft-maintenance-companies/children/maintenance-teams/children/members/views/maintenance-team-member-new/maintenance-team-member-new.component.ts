import { Component, Injector, OnInit } from '@angular/core';
import { MemberNewComponent } from 'src/app/shared/bia-shared/features/members/views/member-new/member-new.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { MaintenanceTeamService } from '../../../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-team-member-new',
  templateUrl: '../../../../../../../../shared/bia-shared/features/members/views/member-new/member-new.component.html',
  styleUrls: ['../../../../../../../../shared/bia-shared/features/members/views/member-new/member-new.component.scss']
})
export class MaintenanceTeamMemberNewComponent extends MemberNewComponent implements OnInit {

  constructor(
    injector: Injector,
    public maintenanceTeamService: MaintenanceTeamService,
  ) {
    super(injector);
  }

  ngOnInit() {
    if (this.maintenanceTeamService.currentMaintenanceTeam!= null) this.teamId = this.maintenanceTeamService.currentMaintenanceTeam.id;
    this.teamTypeId=TeamTypeId.MaintenanceTeam;
    super.ngOnInit();
  }
}
