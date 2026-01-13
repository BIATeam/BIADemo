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
import { SiteService } from 'src/app/features/sites/services/site.service';
import { TeamTypeId } from 'src/app/shared/constants';
import { Permission } from 'src/app/shared/permission';

@Component({
  selector: 'app-site-members-index',
  templateUrl:
    '../../../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/members/views/members-index/members-index.component.html',
  styleUrls: [
    '../../../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-items-index/crud-items-index.component.scss',
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
