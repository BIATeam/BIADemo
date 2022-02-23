import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { MembersIndexComponent } from 'src/app/shared/bia-shared/features/members/views/members-index/members-index.component';
import { MembersSignalRService } from 'src/app/shared/bia-shared/features/members/services/member-signalr.service';
import { MemberOptionsService } from 'src/app/shared/bia-shared/features/members/services/member-options.service';
import { MemberDas } from 'src/app/shared/bia-shared/features/members/services/member-das.service';

@Component({
  selector: 'app-site-members-index',
  templateUrl: '../../../../../../shared/bia-shared/features/members/views/members-index/members-index.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/features/members/views/members-index/members-index.component.scss']
})
export class SiteMembersIndexComponent extends MembersIndexComponent implements OnInit, OnDestroy {
  constructor(
    protected store: Store<AppState>,
    protected router: Router,
    public activatedRoute: ActivatedRoute,
    protected authService: AuthService,
    protected memberDas: MemberDas,
    protected translateService: TranslateService,
    protected biaTranslationService: BiaTranslationService,
    protected membersSignalRService: MembersSignalRService,
    public memberOptionsService: MemberOptionsService,
    public siteService: SiteService,
  ) {
    super(store, router, activatedRoute, authService, memberDas, translateService, biaTranslationService, membersSignalRService, memberOptionsService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.parentIds = ['' + this.siteService.currentSiteId];
  }

  ngOnDestroy()
  {
    super.ngOnDestroy();
  }
}
