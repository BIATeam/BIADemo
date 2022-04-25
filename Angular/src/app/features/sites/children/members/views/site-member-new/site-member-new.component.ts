import { Component, Injector, OnInit } from '@angular/core';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { MemberNewComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-new/member-new.component';
import { TeamTypeId } from 'src/app/shared/constants';

@Component({
  selector: 'app-site-member-new',
  templateUrl: '../../../../../../shared/bia-shared/feature-templates/members/views/member-new/member-new.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/feature-templates/members/views/member-new/member-new.component.scss']
})
export class SiteMemberNewComponent extends MemberNewComponent implements OnInit {

  constructor(
    injector: Injector,
    public siteService: SiteService,
  ) {
    super(injector);
  }

  ngOnInit() {
    this.teamId = this.siteService.currentSiteId;
    this.teamTypeId=TeamTypeId.Site;
    super.ngOnInit();
  }
}
