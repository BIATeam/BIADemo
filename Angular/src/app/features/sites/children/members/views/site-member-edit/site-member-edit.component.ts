import { Component, Injector, OnInit } from '@angular/core';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { MemberEditComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { MemberFormEditComponent } from '../../../../../../shared/bia-shared/feature-templates/members/components/member-form-edit/member-form-edit.component';
import { NgIf, AsyncPipe } from '@angular/common';
import { SpinnerComponent } from '../../../../../../shared/bia-shared/components/spinner/spinner.component';

@Component({
    selector: 'app-site-member-edit',
    templateUrl: '../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.html',
    imports: [MemberFormEditComponent, NgIf, SpinnerComponent, AsyncPipe]
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
