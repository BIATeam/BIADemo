import { Component, Injector, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { MembersIndexComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/members-index/members-index.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { Permission } from 'src/app/shared/permission';

@Component({
  selector: 'app-site-members-index',
  templateUrl: '../../../../../../shared/bia-shared/feature-templates/members/views/members-index/members-index.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/feature-templates/members/views/members-index/members-index.component.scss']
})
export class SiteMembersIndexComponent extends MembersIndexComponent implements OnInit {
  constructor(
    injector: Injector,
    public siteService: SiteService,
    private authService : AuthService
  ) {
    super(injector);
  }

  ngOnInit() {
    this.teamTypeId = TeamTypeId.Site;
    super.ngOnInit();
    this.parentIds = [this.siteService.currentSiteId?.toString()];
  }
  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Site_Member_Update);
    this.canDelete = this.authService.hasPermission(Permission.Site_Member_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Site_Member_Create);
  }
}
