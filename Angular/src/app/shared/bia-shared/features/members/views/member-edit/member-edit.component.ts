import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { update } from '../../store/members-actions';
import { Subscription } from 'rxjs';
import { Member } from '../../model/member';
import { AppState } from 'src/app/store/state';
import { MemberService } from '../../services/member.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MemberOptionsService } from '../../services/member-options.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.scss']
})
export class MemberEditComponent implements OnInit, OnDestroy {
  @Output() displayChange = new EventEmitter<boolean>();
  protected sub = new Subscription();
  public teamId: number;
  public teamTypeId: number;
  
  constructor(
    protected store: Store<AppState>,
    protected router: Router,
    protected activatedRoute: ActivatedRoute,
    public memberOptionsService: MemberOptionsService,
    public memberService: MemberService,
  ) { }

  ngOnInit() {
    this.memberOptionsService.loadAllOptions(this.teamTypeId);
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(memberToUpdate: Member) {
    this.store.dispatch(update({ member: memberToUpdate }));
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }
}
