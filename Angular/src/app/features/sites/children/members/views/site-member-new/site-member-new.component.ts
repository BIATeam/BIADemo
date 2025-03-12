import { Component, Injector, OnInit } from '@angular/core';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { MemberNewComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-new/member-new.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { MemberFormNewComponent } from '../../../../../../shared/bia-shared/feature-templates/members/components/member-form-new/member-form-new.component';
import { AsyncPipe } from '@angular/common';

@Component({
    selector: 'app-site-member-new',
    templateUrl: '../../../../../../shared/bia-shared/feature-templates/members/views/member-new/member-new.component.html',
    imports: [MemberFormNewComponent, AsyncPipe]
})
export class SiteMemberNewComponent
  extends MemberNewComponent
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
  }
}
