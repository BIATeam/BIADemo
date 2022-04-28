import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { FeatureMembersActions } from '../../store/members-actions';
import { Members } from '../../model/member';
import { AppState } from 'src/app/store/state';
import { MemberOptionsService } from '../../services/member-options.service';
import { ActivatedRoute, Router } from '@angular/router';
import { getLastUsersAdded } from 'src/app/domains/bia-domains/user-option/store/user-option.state';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';

@Component({
  selector: 'bia-member-new',
  templateUrl: './member-new.component.html',
  styleUrls: ['./member-new.component.scss']
})
export class MemberNewComponent implements OnInit, OnDestroy {
  public teamId: number;
  public teamTypeId: number;

  protected store: Store<AppState>;
  protected router: Router;
  protected activatedRoute: ActivatedRoute;
  public memberOptionsService: MemberOptionsService;
  protected sub = new Subscription();
  public members : Members;
  protected authService: AuthService;
  canAddFromDirectory = false;
  
  constructor( injector: Injector ) {
    this.store = injector.get<Store<AppState>>(Store);
    this.router = injector.get<Router>(Router);
    this.activatedRoute = injector.get<ActivatedRoute>(ActivatedRoute);
    this.memberOptionsService = injector.get<MemberOptionsService>(MemberOptionsService);
    this.authService = injector.get<AuthService>(AuthService);
  }

  ngOnInit() {
    this.sub = new Subscription();
    this.canAddFromDirectory = this.authService.hasPermission(Permission.User_Add);
    this.members = new Members();
    this.memberOptionsService.loadAllOptions(this.teamTypeId);
    this.sub.add(
      this.store.select(getLastUsersAdded).pipe(skip(1)).subscribe(lastUsersAdded => {
        this.memberOptionsService.refreshUsersOptions();
        this.members.users = lastUsersAdded;
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(membersToCreate: Members) {
    this.store.dispatch(FeatureMembersActions.createMulti({ members: membersToCreate }));
    this.members = new Members();
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.members = new Members();
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
