import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute, Router } from '@angular/router';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { MemberOptionsService } from 'src/app/shared/bia-shared/features/members/services/member-options.service';
import { MemberNewComponent } from 'src/app/shared/bia-shared/features/members/views/member-new/member-new.component';

@Component({
  selector: 'app-site-member-new',
  templateUrl: '../../../../../../shared/bia-shared/features/members/views/member-new/member-new.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/features/members/views/member-new/member-new.component.scss']
})
export class SiteMemberNewComponent extends MemberNewComponent implements OnInit {

  constructor(
    protected store: Store<AppState>,
    protected router: Router,
    protected activatedRoute: ActivatedRoute,
    public memberOptionsService: MemberOptionsService,
    public siteService: SiteService,
  ) {
    super(store, router, activatedRoute, memberOptionsService);
  }

  ngOnInit() {
    if (this.siteService.currentSite!= null) this.teamId = this.siteService.currentSite.id;
    this.teamType=1;
    super.ngOnInit();
  }
}
