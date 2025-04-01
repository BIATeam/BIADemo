import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { MemberFormEditComponent } from 'src/app/shared/bia-shared/feature-templates/members/components/member-form-edit/member-form-edit.component';
import { MemberModule } from 'src/app/shared/bia-shared/feature-templates/members/member.module';
import { MemberEditComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component';
import { TeamTypeId } from 'src/app/shared/constants';

@Component({
  selector: 'app-site-member-edit',
  templateUrl:
    '../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.html',
  imports: [
    MemberModule,
    NgIf,
    AsyncPipe,
    SpinnerComponent,
    MemberFormEditComponent,
  ],
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
