import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute, Router } from '@angular/router';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { MemberEditComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component';
import { MemberOptionsService } from 'src/app/shared/bia-shared/feature-templates/members/services/member-options.service';
import { MemberService } from 'src/app/shared/bia-shared/feature-templates/members/services/member.service';
import { TeamTypeId } from 'src/app/shared/constants';

@Component({
  selector: 'app-site-member-edit',
  templateUrl: '../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.scss']
})
export class SiteMemberEditComponent extends MemberEditComponent implements OnInit {
  constructor(
    protected store: Store<AppState>,
    protected router: Router,
    protected activatedRoute: ActivatedRoute,
    public memberOptionsService: MemberOptionsService,
    public memberService: MemberService,
    public siteService: SiteService,
  ) { 
    super(store, router, activatedRoute, memberOptionsService, memberService);
  }

  ngOnInit() {
    if (this.siteService.currentSite!= null) this.teamId = this.siteService.currentSite.id;
    this.teamTypeId=TeamTypeId.Site;
    super.ngOnInit();
  }
}
