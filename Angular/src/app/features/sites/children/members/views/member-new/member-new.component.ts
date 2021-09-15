import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { create } from '../../store/members-actions';
import { Member } from '../../model/member';
import { AppState } from 'src/app/store/state';
import { MemberOptionsService } from '../../services/member-options.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SiteService } from 'src/app/features/sites/services/site.service';

@Component({
  selector: 'app-member-new',
  templateUrl: './member-new.component.html',
  styleUrls: ['./member-new.component.scss']
})
export class MemberNewComponent implements OnInit {

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    public memberOptionsService: MemberOptionsService,
    public siteService: SiteService,
  ) {}

  ngOnInit() {
    this.memberOptionsService.loadAllOptions();
  }

  onSubmitted(memberToCreate: Member) {
    this.store.dispatch(create({ member: memberToCreate }));
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
