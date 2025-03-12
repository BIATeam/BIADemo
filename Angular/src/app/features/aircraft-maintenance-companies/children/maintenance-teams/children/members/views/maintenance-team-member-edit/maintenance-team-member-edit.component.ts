import { Component, Injector, OnInit } from '@angular/core';
import { MemberEditComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { MaintenanceTeamService } from '../../../../services/maintenance-team.service';
import { MemberModule } from '../../../../../../../../shared/bia-shared/feature-templates/members/member.module';
import { NgIf, AsyncPipe } from '@angular/common';
import { BiaSharedModule } from '../../../../../../../../shared/bia-shared/bia-shared.module';

@Component({
    selector: 'app-maintenance-team-member-edit',
    templateUrl: '../../../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.html',
    imports: [MemberModule, NgIf, BiaSharedModule, AsyncPipe]
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
