import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import {
  MemberEditComponent,
  MemberFormEditComponent,
  MemberModule,
  SpinnerComponent,
} from '@bia-team/bia-ng/shared';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { TeamTypeId } from 'src/app/shared/constants';

@Component({
  selector: 'app-site-member-edit',
  templateUrl:
    '../../../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/members/views/member-edit/member-edit.component.html',
  imports: [MemberModule, AsyncPipe, SpinnerComponent, MemberFormEditComponent],
})
export class SiteMemberEditComponent
  extends MemberEditComponent
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
