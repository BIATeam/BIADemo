import { Component, Injector, OnInit } from '@angular/core';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { MemberNewComponent } from 'src/app/shared/bia-shared/features/members/views/member-new/member-new.component';

@Component({
  selector: 'app-site-member-new',
  templateUrl: '../../../../../../shared/bia-shared/features/members/views/member-new/member-new.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/features/members/views/member-new/member-new.component.scss']
})
export class SiteMemberNewComponent extends MemberNewComponent implements OnInit {

  constructor(
    injector: Injector,
    public siteService: SiteService,
  ) {
    super(injector);
  }

  ngOnInit() {
    if (this.siteService.currentSite!= null) this.teamId = this.siteService.currentSite.id;
    this.teamType=1;
    super.ngOnInit();
  }
}
