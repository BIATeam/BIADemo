import { Component, Injector, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { create } from '../../store/members-actions';
import { Member } from '../../model/member';
import { AppState } from 'src/app/store/state';
import { MemberOptionsService } from '../../services/member-options.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-member-new',
  templateUrl: './member-new.component.html',
  styleUrls: ['./member-new.component.scss']
})
export class MemberNewComponent implements OnInit {
  public teamId: number;
  public teamTypeId: number;

  protected store: Store<AppState>;
  protected router: Router;
  protected activatedRoute: ActivatedRoute;
  public memberOptionsService: MemberOptionsService;
  constructor( injector: Injector ) {
    this.store = injector.get<Store<AppState>>(Store);
    this.router = injector.get<Router>(Router);
    this.activatedRoute = injector.get<ActivatedRoute>(ActivatedRoute);
    this.memberOptionsService = injector.get<MemberOptionsService>(MemberOptionsService);
  }

  ngOnInit() {
    this.memberOptionsService.loadAllOptions(this.teamTypeId);
  }

  onSubmitted(memberToCreate: Member) {
    this.store.dispatch(create({ member: memberToCreate }));
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
