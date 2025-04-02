import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { BiaTableBehaviorControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-behavior-controller/bia-table-behavior-controller.component';
import { BiaTableControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { MemberTableComponent } from 'src/app/shared/bia-shared/feature-templates/members/components/member-table/member-table.component';
import { MembersIndexComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/members-index/members-index.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { Permission } from 'src/app/shared/permission';
import { MemberModule } from '../../../../../../shared/bia-shared/feature-templates/members/member.module';

@Component({
  selector: 'app-site-members-index',
  templateUrl:
    '../../../../../../shared/bia-shared/feature-templates/members/views/members-index/members-index.component.html',
  styleUrls: [
    '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component.scss',
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
      useExisting: SiteService,
    },
  ],
})
export class SiteMembersIndexComponent
  extends MembersIndexComponent
  implements OnInit
{
  constructor(
    injector: Injector,
    public siteService: SiteService
  ) {
    super(injector);
  }

  ngOnInit() {
    this.teamTypeId = TeamTypeId.Site;
    super.ngOnInit();
    this.parentDisplayItemName$ = this.siteService.displayItemName$;
    this.memberService.parentService = this.siteService;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(
      Permission.Site_Member_Update
    );
    this.canDelete = this.authService.hasPermission(
      Permission.Site_Member_Delete
    );
    this.canAdd = this.authService.hasPermission(Permission.Site_Member_Create);
    this.canSave = this.authService.hasPermission(Permission.Site_Member_Save);
  }
}
