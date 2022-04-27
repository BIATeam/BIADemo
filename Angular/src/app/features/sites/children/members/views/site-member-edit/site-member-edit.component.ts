import { Component, Injector, OnInit } from '@angular/core';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { MemberEditComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component';
import { TeamTypeId } from 'src/app/shared/constants';

@Component({
  selector: 'app-site-member-edit',
  templateUrl: '../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.scss']
})
export class SiteMemberEditComponent extends MemberEditComponent implements OnInit {
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
