import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import {
  MemberFormNewComponent,
  MemberModule,
  MemberNewComponent,
} from 'bia-ng/shared';
import { TeamTypeId } from 'src/app/shared/constants';
import { MaintenanceTeamService } from '../../../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-team-member-new',
  templateUrl:
    '../../../../../../../../../../node_modules/bia-ng/templates/feature-templates/members/views/member-new/member-new.component.html',
  imports: [MemberModule, AsyncPipe, MemberFormNewComponent],
})
export class MaintenanceTeamMemberNewComponent
  extends MemberNewComponent
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
