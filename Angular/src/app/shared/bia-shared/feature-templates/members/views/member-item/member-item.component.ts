import { Component, OnInit, OnDestroy, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { getCurrentMember} from '../../store/member.state';
import { Member } from '../../model/member';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute } from '@angular/router';
import { MemberService } from '../../services/member.service';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { first } from 'rxjs/operators';

@Component({
  templateUrl: './member-item.component.html',
  styleUrls: ['./member-item.component.scss']
})
export class MemberItemComponent implements OnInit, OnDestroy {
  member$: Observable<Member>;
  protected sub = new Subscription();
  protected store: Store<AppState>;
  protected activatedRoute: ActivatedRoute;
  public memberService: MemberService;
  protected layoutService: BiaClassicLayoutService;

  constructor(injector: Injector) { 
    this.store = injector.get<Store<AppState>>(Store);
    this.activatedRoute = injector.get<ActivatedRoute>(ActivatedRoute);
    this.memberService = injector.get<MemberService>(MemberService);
    this.layoutService = injector.get<BiaClassicLayoutService>(BiaClassicLayoutService);
  }

  ngOnInit() {
    this.memberService.currentMemberId = this.activatedRoute.snapshot.params.memberId;
    this.sub.add
      (
        this.store.select(getCurrentMember).subscribe((member) => {
          if (member?.user) {
            this.activatedRoute.data.pipe(first()).subscribe(routeData => {
              routeData['breadcrumb'] = member.user.display;
            });
            this.layoutService.refreshBreadcrumb();
          }
        })
      );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }
}
