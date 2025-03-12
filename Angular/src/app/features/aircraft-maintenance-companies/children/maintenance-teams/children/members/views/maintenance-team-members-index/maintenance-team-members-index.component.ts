import { Component, Injector, OnInit } from '@angular/core';
import { MembersIndexComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/members-index/members-index.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { Permission } from 'src/app/shared/permission';
import { MaintenanceTeamService } from '../../../../services/maintenance-team.service';
import { NgClass, NgIf, AsyncPipe } from '@angular/common';
import { BiaSharedModule } from '../../../../../../../../shared/bia-shared/bia-shared.module';
import { PrimeTemplate } from 'primeng/api';
import { MemberModule } from '../../../../../../../../shared/bia-shared/feature-templates/members/member.module';
import { TranslateModule } from '@ngx-translate/core';

@Component({
    selector: 'app-maintenance-team-members-index',
    templateUrl: '../../../../../../../../shared/bia-shared/feature-templates/members/views/members-index/members-index.component.html',
    styleUrls: [
        '../../../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component.scss',
    ],
    imports: [NgClass, BiaSharedModule, PrimeTemplate, NgIf, MemberModule, AsyncPipe, TranslateModule]
})
export class MaintenanceTeamMembersIndexComponent
  extends MembersIndexComponent
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

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(
      Permission.MaintenanceTeam_Member_Update
    );
    this.canDelete = this.authService.hasPermission(
      Permission.MaintenanceTeam_Member_Delete
    );
    this.canAdd = this.authService.hasPermission(
      Permission.MaintenanceTeam_Member_Create
    );
  }
}
