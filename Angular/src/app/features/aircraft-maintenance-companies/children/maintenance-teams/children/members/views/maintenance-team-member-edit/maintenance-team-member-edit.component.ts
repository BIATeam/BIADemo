import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute, Router } from '@angular/router';
import { MemberEditComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component';
import { MemberOptionsService } from 'src/app/shared/bia-shared/feature-templates/members/services/member-options.service';
import { MemberService } from 'src/app/shared/bia-shared/feature-templates/members/services/member.service';
import { TeamTypeId } from 'src/app/shared/constants';
import { MaintenanceTeamService } from '../../../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-team-member-edit',
  templateUrl: '../../../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.html',
  styleUrls: ['../../../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.scss']
})
export class MaintenanceTeamMemberEditComponent extends MemberEditComponent implements OnInit {
  constructor(
    protected store: Store<AppState>,
    protected router: Router,
    protected activatedRoute: ActivatedRoute,
    public memberOptionsService: MemberOptionsService,
    public memberService: MemberService,
    public maintenanceTeamService: MaintenanceTeamService,
  ) { 
    super(store, router, activatedRoute, memberOptionsService, memberService);
  }

  ngOnInit() {
    this.teamId = this.maintenanceTeamService.currentMaintenanceTeamId;
    this.teamTypeId=TeamTypeId.MaintenanceTeam;
    super.ngOnInit();
  }
}
