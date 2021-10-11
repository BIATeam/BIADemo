import { Component, OnInit, OnDestroy } from '@angular/core';
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
  private sub = new Subscription();

  constructor(private store: Store<AppState>,
    private route: ActivatedRoute,
    public memberService: MemberService,
    private layoutService: BiaClassicLayoutService) { }

  ngOnInit() {
    this.memberService.currentMemberId = this.route.snapshot.params.memberId;
    this.sub.add
      (
        this.store.select(getCurrentMember).subscribe((member) => {
          if (member?.user) {
            this.route.data.pipe(first()).subscribe(routeData => {
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
