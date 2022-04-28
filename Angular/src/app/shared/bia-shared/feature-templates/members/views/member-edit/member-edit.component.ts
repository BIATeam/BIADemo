import { Component, OnInit, Output, EventEmitter, OnDestroy, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { FeatureMembersActions } from '../../store/members-actions';
import { Subscription } from 'rxjs';
import { Member } from '../../model/member';
import { AppState } from 'src/app/store/state';
import { MemberService } from '../../services/member.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MemberOptionsService } from '../../services/member-options.service';
import { getLastUsersAdded } from 'src/app/domains/bia-domains/user-option/store/user-option.state';
import { skip } from 'rxjs/operators';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';

@Component({
  selector: 'bia-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.scss']
})
export class MemberEditComponent implements OnInit, OnDestroy {
  @Output() displayChange = new EventEmitter<boolean>();
  protected sub = new Subscription();
  public teamId: number;
  public teamTypeId: number;
  protected store: Store<AppState>;
  protected router: Router;
  protected activatedRoute: ActivatedRoute;
  public memberOptionsService: MemberOptionsService;
  public memberService: MemberService;
  protected authService: AuthService;
  canAddFromDirectory = false;

  constructor( injector: Injector  ) {
    this.memberService = injector.get<MemberService>(MemberService);
    this.store = injector.get<Store<AppState>>(Store);
    this.router = injector.get<Router>(Router);
    this.activatedRoute = injector.get<ActivatedRoute>(ActivatedRoute);
    this.memberOptionsService = injector.get<MemberOptionsService>(MemberOptionsService);
    this.authService = injector.get<AuthService>(AuthService);
   }

  ngOnInit() {
    this.sub = new Subscription();
    this.canAddFromDirectory = this.authService.hasPermission(Permission.User_Add);
    this.memberOptionsService.loadAllOptions(this.teamTypeId);
    this.sub.add(
      this.store.select(getLastUsersAdded).pipe(skip(1)).subscribe(event => {
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
