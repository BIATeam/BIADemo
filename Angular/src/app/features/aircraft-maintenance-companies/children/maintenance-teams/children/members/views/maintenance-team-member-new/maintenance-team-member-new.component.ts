import { Component, Injector, OnInit } from '@angular/core';
import { MaintenanceTeamService } from '../../../../services/maintenance-team.service';
import { MemberNewComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-new/member-new.component';
import { TeamTypeId } from 'src/app/shared/constants';

@Component({
  selector: 'app-maintenance-team-member-new',
  templateUrl: '../../../../../../../../shared/bia-shared/feature-templates/members/views/member-new/member-new.component.html',
})
export class MaintenanceTeamMemberNewComponent extends MemberNewComponent implements OnInit {

  constructor(
    injector: Injector,
    public maintenanceTeamService: MaintenanceTeamService,
  ) {
    super(injector);
  }

  ngOnInit() {
    this.teamTypeId=TeamTypeId.MaintenanceTeam;
    super.ngOnInit();
  }
}
