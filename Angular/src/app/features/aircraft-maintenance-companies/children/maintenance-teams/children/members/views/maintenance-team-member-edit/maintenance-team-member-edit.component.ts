import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import {
  MemberEditComponent,
  MemberFormEditComponent,
  MemberModule,
  SpinnerComponent,
} from 'bia-ng/shared';
import { TeamTypeId } from 'src/app/shared/constants';
import { MaintenanceTeamService } from '../../../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-team-member-edit',
  templateUrl:
    '../../../../../../../../../../node_modules/bia-ng/templates/feature-templates/members/views/member-edit/member-edit.component.html',
  imports: [
    MemberModule,
    NgIf,
    AsyncPipe,
    SpinnerComponent,
    MemberFormEditComponent,
  ],
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
