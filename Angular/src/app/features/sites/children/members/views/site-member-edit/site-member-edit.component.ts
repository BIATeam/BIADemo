import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import {
  MemberEditComponent,
  MemberFormEditComponent,
  MemberModule,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { TeamTypeId } from 'src/app/shared/constants';

@Component({
  selector: 'app-site-member-edit',
  templateUrl:
    '../../../../../../../../packages/bia-ng/shared/feature-templates/members/views/member-edit/member-edit.component.html',
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
