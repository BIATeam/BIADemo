import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { create } from '../../store/members-actions';
import { Member } from '../../model/member';
import { AppState } from 'src/app/store/state';
import { MemberOptionsService } from '../../services/member-options.service';
import { ActivatedRoute, Router } from '@angular/router';
import { getUserOptionsChangeCount } from 'src/app/domains/bia-domains/user-option/store/user-option.state';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-member-new',
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
  
  constructor( injector: Injector ) {
    this.store = injector.get<Store<AppState>>(Store);
    this.router = injector.get<Router>(Router);
    this.activatedRoute = injector.get<ActivatedRoute>(ActivatedRoute);
    this.memberOptionsService = injector.get<MemberOptionsService>(MemberOptionsService);
  }

  ngOnInit() {
    this.sub = new Subscription();
    this.memberOptionsService.loadAllOptions(this.teamTypeId);
    this.sub.add(
      this.store.select(getUserOptionsChangeCount).subscribe(event => {
        this.memberOptionsService.refreshUsersOptions();
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(memberToCreate: Member) {
    this.store.dispatch(create({ member: memberToCreate }));
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
