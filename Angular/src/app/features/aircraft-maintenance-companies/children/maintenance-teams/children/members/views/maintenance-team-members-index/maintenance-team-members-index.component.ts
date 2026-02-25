import { AsyncPipe, NgClass } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import {
  BiaTableBehaviorControllerComponent,
  BiaTableComponent,
  BiaTableControllerComponent,
  BiaTableHeaderComponent,
  CrudItemService,
  MemberModule,
  MembersIndexComponent,
  MemberTableComponent,
} from '@bia-team/bia-ng/shared';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { TeamTypeId } from 'src/app/shared/constants';
import { Permission } from 'src/app/shared/permission';
import { MaintenanceTeamService } from '../../../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-team-members-index',
  templateUrl:
    '../../../../../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/members/views/members-index/members-index.component.html',
  styleUrls: [
    '../../../../../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-items-index/crud-items-index.component.scss',
  ],
  imports: [
    NgClass,
    PrimeTemplate,
    MemberModule,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
    MemberTableComponent,
  ],
  providers: [
    {
      provide: CrudItemService,
      useExisting: MaintenanceTeamService,
    },
  ],
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
    this.memberService.parentService = this.maintenanceTeamService;
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
