import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute } from '@angular/router';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { MemberService } from 'src/app/shared/bia-shared/features/members/services/member.service';
import { MemberItemComponent } from 'src/app/shared/bia-shared/features/members/views/member-item/member-item.component';

@Component({
  templateUrl: '../../../../../../shared/bia-shared/features/members/views/member-item/member-item.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/features/members/views/member-item/member-item.component.scss']
})
export class SiteMemberItemComponent extends MemberItemComponent implements OnInit, OnDestroy {
  constructor(protected store: Store<AppState>,
    protected route: ActivatedRoute,
    public memberService: MemberService,
    protected layoutService: BiaClassicLayoutService) { 
      super (store, route, memberService, layoutService);
    }

    ngOnInit()
    {
      super.ngOnInit();
    }

    ngOnDestroy()
    {
      super.ngOnDestroy();
    }
}
