import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { BiaTableBehaviorControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-behavior-controller/bia-table-behavior-controller.component';
import { BiaTableControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { MemberTableComponent } from 'src/app/shared/bia-shared/feature-templates/members/components/member-table/member-table.component';
import { MembersIndexComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/members-index/members-index.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { Permission } from 'src/app/shared/permission';
import { MemberModule } from '../../../../../../../../shared/bia-shared/feature-templates/members/member.module';
import { MaintenanceTeamService } from '../../../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-team-members-index',
  templateUrl:
    '../../../../../../../../shared/bia-shared/feature-templates/members/views/members-index/members-index.component.html',
  styleUrls: [
    '../../../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component.scss',
  ],
  imports: [
    NgClass,
    PrimeTemplate,
    NgIf,
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
