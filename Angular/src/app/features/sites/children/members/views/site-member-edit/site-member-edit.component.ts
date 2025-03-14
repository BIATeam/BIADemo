import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { Component, Injector, OnInit } from '@angular/core';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { MemberEditComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { MemberModule } from '../../../../../../shared/bia-shared/feature-templates/members/member.module';
import { NgIf, AsyncPipe } from '@angular/common';
import { BiaSharedModule } from '../../../../../../shared/bia-shared/bia-shared.module';

@Component({
    selector: 'app-site-member-edit',
    templateUrl: '../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.html',
    imports: [MemberModule, NgIf, BiaSharedModule, AsyncPipe, SpinnerComponent]
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
