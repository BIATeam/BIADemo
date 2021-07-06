import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import * as fromRoles from 'src/app/domains/role/store/role.state';
import { Member } from '../../model/member';
import { getCurrentMember, getMemberLoadingGet, getDisplayEditDialog } from '../../store/member.state';
import { update, closeDialogEdit } from '../../store/members-actions';
import { AppState } from 'src/app/store/state';
import { Role } from 'src/app/domains/role/model/role';

@Component({
  selector: 'app-member-edit-dialog',
  templateUrl: './member-edit-dialog.component.html',
  styleUrls: ['./member-edit-dialog.component.scss']
})
export class MemberEditDialogComponent implements OnInit, OnDestroy {
  loading$: Observable<boolean>;
  member$: Observable<Member>;
  allRoles$: Observable<Role[]>;
  display = false;
  private sub = new Subscription();
  @Output() displayChange = new EventEmitter<boolean>();

  constructor(private store: Store<AppState>) {}

  ngOnInit() {
    this.loading$ = this.store.select(getMemberLoadingGet).pipe();
    this.allRoles$ = this.store.pipe(select(fromRoles.getAllRoles)).pipe();
    this.member$ = this.store.select(getCurrentMember).pipe();
    this.sub.add(
      this.store
        .select(getDisplayEditDialog)
        .pipe()
        .subscribe((x) => (this.display = x))
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(memberToUpdate: Member) {
    this.store.dispatch(update({ member: memberToUpdate }));
    this.close();
  }

  onCancelled() {
    this.close();
  }

  close() {
    this.store.dispatch(closeDialogEdit());
  }
}
