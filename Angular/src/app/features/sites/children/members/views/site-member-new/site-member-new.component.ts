import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import {
  MemberFormNewComponent,
  MemberModule,
  MemberNewComponent,
} from '@bia-team/bia-ng/shared';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { TeamTypeId } from 'src/app/shared/constants';

@Component({
  selector: 'app-site-member-new',
  templateUrl:
    '../../../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/members/views/member-new/member-new.component.html',
  imports: [MemberModule, AsyncPipe, MemberFormNewComponent],
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
