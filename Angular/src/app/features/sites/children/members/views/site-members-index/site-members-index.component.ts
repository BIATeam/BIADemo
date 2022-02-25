import { Component, Injector, OnInit } from '@angular/core';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { MembersIndexComponent } from 'src/app/shared/bia-shared/features/members/views/members-index/members-index.component';
import { TeamTypeId } from 'src/app/shared/constants';

@Component({
  selector: 'app-site-members-index',
  templateUrl: '../../../../../../shared/bia-shared/features/members/views/members-index/members-index.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/features/members/views/members-index/members-index.component.scss']
})
export class SiteMembersIndexComponent extends MembersIndexComponent implements OnInit {
  constructor(
    injector: Injector,
    public siteService: SiteService,
  ) {
    super(injector);
  }

  ngOnInit() {
    this.teamTypeId=TeamTypeId.Site;
    super.ngOnInit();
    this.parentIds = ['' + this.siteService.currentSiteId];
  }
}
