import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { FeatureMembersActions } from '../../store/members-actions';
import { Subscription } from 'rxjs';
import { Member } from '../../model/member';
import { AppState } from 'src/app/store/state';
import { MemberService } from '../../services/member.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MemberOptionsService } from '../../services/member-options.service';
import { getLastUsersAdded } from 'src/app/domains/bia-domains/user-option/store/user-option.state';

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
    this.sub = new Subscription();
    this.memberOptionsService.loadAllOptions(this.teamTypeId);
    this.sub.add(
      this.store.select(getLastUsersAdded).subscribe(event => {
        this.memberOptionsService.refreshUsersOptions();
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(memberToUpdate: Member) {
    this.store.dispatch(FeatureMembersActions.update({ member: memberToUpdate }));
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }
}
